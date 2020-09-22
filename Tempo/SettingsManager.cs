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

        public static bool IsLoaded { get; private set; }

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
                IsLoaded = true;
            }
            catch
            {
                throw new Exception("Failed to load settings file.");
            }
        }

        public static void HideTemplate(string groupName, string templateName)
        {
            foreach (var group in TemplateGroups)
            {
                if (group.Name == groupName)
                {
                    group.HideTemplate(templateName);
                    Save();
                    Logger.Info($"{groupName}/{templateName} no longer will be parsed as template");
                    return;
                }
            }
            
            Logger.Warning($"{groupName} not found.");
        }

        public static void RemoveFromHidden(string groupName, string templateName)
        {
            foreach (var templateGroup in TemplateGroups)
            {
                if (templateGroup.Name == groupName)
                {
                    templateGroup.RemoveFromHidden(templateName);
                    Save();
                    Logger.Info($"{groupName}/{templateName} will be parsed is a template now.");
                    return;
                }
            }
            
            Logger.Warning($"{groupName} not found.");
        }
        
        public static void ListGroups()
        {
            foreach (var group in TemplateGroups)
            {
                var absolutePath = PathConverter.ToAbsolutePath(group.Path);
                
                if (string.IsNullOrEmpty(group.Name))
                {
                    Logger.Warning($"Template group at \"{group.Path}\" does not have name.");
                    Logger.Warning("You will not able to build from templates in this folder. You can fix it in settings.json");
                }
                var templatesDir = new DirectoryInfo(absolutePath);
                if (templatesDir.Exists)
                {
                    // Check if language is specified
                    var groupLanguage = String.IsNullOrEmpty(group.Language)
                        ? "Not Specified"
                        : group.Language;
                    
                    Logger.Info($"Template group {group.Name} at {group.Path}");
                    foreach (var directory in templatesDir.GetDirectories())
                    {
                        var ignoredMessage = "";
                        if (group.Hidden != null && group.Hidden.Count > 0 && group.Hidden.Contains(directory.Name))
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                            ignoredMessage = group.Hidden.Contains(directory.Name) ? "[IGNORED]": "";
                        }
                        
                        Logger.Default($"\t{ignoredMessage} Template: {directory.Name}, language: { groupLanguage }, using: {group.Name}/{directory.Name}");  
                        Console.ResetColor();
                    }
                }
                else
                {
                    Logger.Warning($"Template group \"{group.Name ?? "~No Name~"}\": Path \"{group.Path}\" is invalid");
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
                    throw new ArgumentException($"Template folder with name \"{newTemplateGroup.Name}\" already exists!");
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
        public List<string> Hidden { get; private set; } // Names that are ignored

        [JsonProperty]
        public string Path { get; private set; } // Path to the folder with templates

        public void HideTemplate(string name)
        {
            Hidden ??= new List<string>();
            
            if (!Hidden.Contains(name))
                Hidden.Add(name);
        }

        public void RemoveFromHidden(string name)
        {
            Hidden ??= new List<string>();

            Hidden.Remove(name);
        }
        public TemplateGroup(string language, string name, string path)
        {
            if (language != null)
                Language = language;
            Name = name;
            Path = path;
            Hidden = new List<string>();
        }

    }
}