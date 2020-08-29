using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace Templater
{
    // Enum for checking languages 
    public enum Language
    {
        Node
    }
    public class Creator
    {
        private static readonly string[] KnownLanguages = new[]
        {
            "node", // NodeJS
            "python",
            "no-lang" // Template Without language
        };

        private Settings _settings;

        #region Settings
        
        public bool LoadSettings()
        {
            // Getting the settings file.
            _settings = GetSettings();

            return _settings != null;
        }

        private Settings GetSettings()
        {
            var pathToSettingsFile = AppDomain.CurrentDomain.BaseDirectory + "/settings.json";
            if (!File.Exists(pathToSettingsFile))
            {
                return null;
            }

            var json = File.ReadAllText(pathToSettingsFile);
            var settings = JsonSerializer.Deserialize<Settings>(json);
            
            return settings;
        }

        #endregion
        
        public void ShowTemplates()
        {
            foreach (var template in _settings.Templates)
            {
                template.Path = ToFullPath(template.Path);
                
                var templatesDir = new DirectoryInfo(template.Path);
                if (templatesDir.Exists)
                {
                    foreach (var directory in templatesDir.GetDirectories())
                    {
                        Logger.Default($"{template.Language}:{directory.Name}");   
                    }
                }
                else
                {
                    Logger.Warning($"{template.Language}: Path {template.Path} is invalid");
                }
            }
        }
        
        private string ToFullPath(string path)
        {
            if (path == null)
            {
                throw new NullReferenceException();
            }
            
            if (path.StartsWith("@local"))
            {
                path = path.Replace("@local",AppDomain.CurrentDomain.BaseDirectory);
            }

            return path;
        }
        

        public void NewProject(Arguments args)
        {
            if (_settings.Templates.Count < 1)
            {
                Logger.Warning("No templates in settings file.");
                Logger.Info("Check here how to write your settings file: ");
                // TODO: Link to the GitHub Repo
                return;
            }

            // Check in templates if requested language exists. If so, try to parse language and create new project
            foreach (var template in _settings.Templates)
            {
                if (template.Language == args.Language) // Check if requested language exists in settings 
                {
                    if (!KnownLanguages.Contains(template.Language))
                    {
                        Logger.Error($"Unknown template language {template.Language}");
                        // TODO: Post link to the GitHub and how to create templates.
                        return;
                    }
                    
                    template.Path = ToFullPath(template.Path);
                    
                    if (Directory.Exists($"{template.Path}/{args.Template}")) // if Directory with template exists
                    {
                        Logger.Info($"Template for {args.Template} found in {template.Path}");
                        
                        var projectRootPath = CreateProjectFromTemplate(args.ProjectName, template.Path, args.Template);

                        if (projectRootPath != null) // Project Directory created.
                        {
                            switch (template.Language)
                            {
                                case "node":
                                {
                                    TryInstallDependencies(Language.Node, projectRootPath);
                                    Logger.Info("NodeJS project created.");
                                    break;
                                }
                                case "no-lang": // Template without language (Will not try to install dependencies)
                                {
                                    Logger.Info("No-Language project created.");
                                    break;
                                }
                                case "python":
                                {
                                    Logger.Info("Python project created.");
                                    break; 
                                }
                            }
                        }
                        return; // Exit after creating project
                    }
                }
            }
            Logger.Error($"Language \"{args.Language}\" or template \"{args.Template}\" not found in settings file.");
        }
        
        // Returns path to the project root
        private string CreateProjectFromTemplate(string projectName, string templateDirectory, string template)
        {
            var projectDirectory = CreateProjectDirectory(projectName);
            var templatePath = $"{templateDirectory}/{template}";
            
            if (!Directory.Exists(templatePath))
            {
                Logger.Error($"Template \"{template}\" not found.");
                return null;
            }
            
            // Copy files from template to the project root
            CopyFromTemplate($"{templateDirectory}/{template}",projectDirectory.FullName);

            return projectDirectory.FullName;
        }

        private DirectoryInfo CreateProjectDirectory(string projectName)
        {
            // Prompting the user for the root directory for the project
            var projectDirectory = GetProjectDirectory();
            
            // Creating project root directory
            var projectRoot = projectDirectory.FullName + "/" + projectName;
            projectDirectory.CreateSubdirectory(projectName);
            
            return new DirectoryInfo(projectRoot);
        }
        
        private void TryInstallDependencies(Language lang, string projectPath)
        {
            if (lang == Language.Node)
            {
                var packageFilePath = $"{projectPath}/package.json";
                
                if (!File.Exists(packageFilePath))
                {
                    Logger.Warning("No package.json in template. Dependencies not installed.");
                    return;
                }
                
                Logger.Info("Detected package.json");
                
                Directory.SetCurrentDirectory(projectPath);
                
                try
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Logger.Info("Installing Dependencies...");
                    Thread.Sleep(1000);
               
                    // Start cmd.exe on windows instead of just npm
                    var program = Environment.OSVersion.Platform == PlatformID.Unix ? "npm" : "cmd.exe";
                    var programArguments =
                        Environment.OSVersion.Platform == PlatformID.Unix ? "install" : "npm install";
                    
                    var npm = Process.Start(program, programArguments);
                    npm.WaitForExit();
                }
                catch (Win32Exception)
                {
                    Logger.Warning("Failed to install dependencies.");
                }
            }
        }
        
        private DirectoryInfo GetProjectDirectory()
        {
            while (true)
            {
                Console.Write("Directory for the project (Current Directory by default): ");
                var projectDir = Console.ReadLine();
                
                if (projectDir.Trim() == "")
                {
                    Logger.Info("Creating project in current directory");
                    return new DirectoryInfo(Directory.GetCurrentDirectory());
                }

                if (!Directory.Exists(projectDir.Trim()))
                {
                    Logger.Error($"No directory in \"{projectDir}\"");
                    continue;
                }
                
                return  new DirectoryInfo(projectDir);
            }
        }
        
        private static void CopyFromTemplate(string templatePath, string destinationPath)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(templatePath);

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                var temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, true);
            }

            // Copy subdirectories
            foreach (var subdir in dirs)
            {
                var temppath = Path.Combine(destinationPath, subdir.Name);
                CopyFromTemplate(subdir.FullName, temppath);
            }
        }
    }
}