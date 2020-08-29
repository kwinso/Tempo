using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        public void NewProject(Arguments args)
        {
            // Getting the settings file.
            var settings = GetSettings();

            /* Checking for invalid setting file */
            if (settings == null)
            {
                Logger.Error("No settings file found.");
                return;
            }
            if (settings.Templates.Count < 1)
            {
                Logger.Warning("No templates in settings file.");
                Logger.Info("Check here how to write your settings file: ");
                // TODO: Link to the GitHub Repo
                return;
            }

            // Check in templates if requested language exists. If so, try to parse language and create new project
            foreach (var template in settings.Templates)
            {
                if (template.Language == args.Language) // Check if requested language exists in settings 
                {
                    if (Directory.Exists($"{template.Path}/{args.Template}")) // if Directory with template exists
                    {
                        Logger.Info($"Template for {args.Template} found in {template.Path}");
                        
                        if (template.Language == "node")
                        {
                            CreateNodeProject(args.ProjectName, template.Path, args.Template);
                            return;
                        }
                        else if (template.Language == "no-lang")
                        {
                            // TODO: Just copy from the template without any additional operations
                            throw new NotImplementedException("In development.");
                        }
                        else
                        {
                            Logger.Error($"Unknown template language {template.Language}");
                            // TODO: Post link to the GitHub and how to create templates.
                            return;
                        }
                    }
                }
            }
            Logger.Error($"Language \"{args.Language}\" or template \"{args.Template}\" not found in settings file.");
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

        private void CreateNodeProject(string projectName, string templateDirectory, string template)
        {
            var projectDirectory = CreateProjectDirectory(projectName);
            var templatePath = $"{templateDirectory}/{template}";
            
            if (!Directory.Exists(templatePath))
            {
                Logger.Error($"Template \"{template}\" not found.");
                return;
            }
            
            // Copy files from template to the project root
            CopyFromTemplate(
                $"{templateDirectory}/{template}",
                projectDirectory.FullName
            );

            TryInstallDependencies(Language.Node, projectDirectory.FullName);
            
            Logger.Info("Project Created.");
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
                
                Logger.Default("package.json detected.");
                
                Directory.SetCurrentDirectory(projectPath);
                
                try
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Logger.Info("Installing Dependencies...");
                    Thread.Sleep(1000);
                    
                    var npmPsi = new ProcessStartInfo()
                    {
                        FileName = "npm",
                        Arguments = "install",
                        RedirectStandardOutput = false,
                        RedirectStandardError = false,
                    };
                    
                    var npm = Process.Start(npmPsi);
                    npm.WaitForExit();
                }
                catch (Win32Exception)
                {
                    Logger.Warning("Failed to install dependencies.");
                }
            }
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

        private DirectoryInfo GetProjectDirectory()
        {
            while (true)
            {
                Logger.Default("Directory for the project (Current Directory by default): ");
                var projectDir = Console.ReadLine();
                if (projectDir.Trim() == "")
                {
                    Console.WriteLine("Creating project in current directory");
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