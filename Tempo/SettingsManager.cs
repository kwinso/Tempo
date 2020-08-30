using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Tempo
{
    public static class SettingsManager
    {
        private static SettingsFile _settings;

        private static string PathToSettingsFile => AppDomain.CurrentDomain.BaseDirectory + "/settings.json";

        public static IReadOnlyList<TemplateGroup> TemplateGroups => _settings.TemplateGroups.AsReadOnly();

        // This method returns true if file successfully loaded
        public static void Load()
        {
            if (!File.Exists(PathToSettingsFile))
            {
                throw new Exception("Settings file does not exists.");
            }

            try
            {
                var json = File.ReadAllText(PathToSettingsFile);
                _settings =  JsonConvert.DeserializeObject<SettingsFile>(json);
            }
            catch
            {
                throw new Exception("Failed to load settings file.");
            }
        }
        
        public static void ListGroups()
        {
            foreach (var template in TemplateGroups)
            {
                var absolutePath = PathConverter.ToAbsolutePath(template.Path);
                
                if (string.IsNullOrEmpty(template.Name))
                {
                    Logger.Warning($"Template group at \"{template.Path}\" does not have name.");
                    Logger.Warning("You will not able to build from templates in this folder. You can fix it in settings.json");
                }
                var templatesDir = new DirectoryInfo(absolutePath);
                if (templatesDir.Exists)
                {
                    Logger.Info($"Template group {template.Name} at {template.Path}");
                    foreach (var directory in templatesDir.GetDirectories())
                    {
                        Logger.Default($"\tTemplate: {directory.Name}, language: { template.Language ?? "Not Specified" }");   
                    }
                }
                else
                {
                    Logger.Warning($"Template group \"{template.Name ?? "~No Name~"}\": Path \"{template.Path}\" is invalid");
                }
            }
        }

        public static void AddNewTemplateGroup(TemplateGroup newTemplateGroup)
        {
            if (String.IsNullOrEmpty(newTemplateGroup.Name))
            {
                throw new NullReferenceException("Name of a template cannot be empty.");
            }
            // Check if name already in use
            foreach (var template in TemplateGroups)
            {
                if (template.Name == newTemplateGroup.Name)
                {
                    Logger.Error($"Template folder with name \"{newTemplateGroup.Name}\" already exists!");
                    return;
                }
            }
            _settings.Update(newTemplateGroup);
            Save();
        }

        public static void RemoveTemplateGroup(string groupName)
        {
            if (TemplateGroups.Count(x => x.Name == groupName) == 0)
            {
                Logger.Warning($"Template group with name \"{groupName}\" does not exist.");
                return;
            }
            
            _settings.DeleteTemplate(groupName);
            Save();
        }

        private static void Save()
        {
            var json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
            File.WriteAllText(PathToSettingsFile, json);
            Logger.Info("Saved.");
        }
    }
    

    public class SettingsFile
    {
        [JsonProperty]
        public List<TemplateGroup> TemplateGroups { get; private set; } // All available templates
        
        public void Update(TemplateGroup templateGroup)
        {
            TemplateGroups.Add(templateGroup);
        }

        // Filter all directories with name to delete and update the list
        public void DeleteTemplate(string name)
        {
            var filteredTemplates = TemplateGroups.Where(x => x.Name != name);
            TemplateGroups = filteredTemplates.ToList();
        }
    }
    
    public sealed class TemplateGroup
    {
        [JsonProperty]
        public string Language { get; private set; } // Language of templates, e.g. NodeJS, Python
        
        [JsonProperty]
        public string Name { get; private set; } // Of the template folder
        
        [JsonProperty]
        public string Path { get; private set; } // Path to the folder with templates

        
        public TemplateGroup(string language, string name, string path)
        {
            Language = language;
            Name = name;
            Path = path;
        }

    }
}