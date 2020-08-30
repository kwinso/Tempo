using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Tempo
{
    // Enum for checking languages 
    public enum Language
    {
        Node
    }
    public class Creator
    {
        public void NewProject(TemplateInfo args)
        {
            if (SettingsManager.TemplateGroups.Count < 1)
            {
                throw new NullReferenceException("No templates in settings file.");
            }
            
            // Check in templates if requested language exists. If so, try to parse language and create new project
            foreach (var group in SettingsManager.TemplateGroups)
            {
                if (group.Name == args.GroupName) // Check if requested language exists in settings 
                {
                    if (String.IsNullOrEmpty(group.Path))
                    {
                        throw new Exception($"Template group \"{group.Name}\" does not have \"Path\" property.");
                    }
                    
                    var absolutePath = PathConverter.ToAbsolutePath(group.Path); 
                    
                    if (Directory.Exists($"{absolutePath}/{args.Template}")) // if Directory with template exists
                    {
                        Logger.Info($"Template \"{args.Template}\" found in {group.Name} - {absolutePath}");
                        
                        var projectRootPath = CreateProjectFromTemplate(args.ProjectName, absolutePath, args.Template);

                        if (projectRootPath != null && group.Language != null) // Project Directory created.
                        {
                            switch (group.Language)
                            {
                                case "node":
                                {
                                    TryInstallDependencies(Language.Node, projectRootPath);
                                    Logger.Info("NodeJS project created.");
                                    break;
                                }
                                default:
                                {
                                    Logger.Info("Project created from template.");
                                    break;
                                }
                            }
                        }
                        return; // Exit after creating project
                    }
                }
            }
            throw new Exception($"Group \"{args.GroupName}\" or template \"{args.Template}\" not found in settings file.");
        }
        
        // Returns path to the project root
        private string CreateProjectFromTemplate(string projectName, string templateDirectory, string template)
        {
            var projectDirectory = CreateProjectDirectory(projectName);
            var templatePath = $"{templateDirectory}/{template}";
            
            if (!Directory.Exists(templatePath))
            {
                throw new Exception($"Template \"{template}\" not found.");
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

                    // Run npm in another shell on Windows
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        var cmdPsi = new ProcessStartInfo()
                        {
                            FileName = "cmd",
                            RedirectStandardInput = true,
                            WorkingDirectory = projectPath
                        };
                        var cmd = Process.Start(cmdPsi);
                        
                        cmd.StandardInput.WriteLine($"npm install & exit");
                        
                        Logger.Info("Entered new cmd to install packages.");
                        
                        cmd.WaitForExit();
                    }
                    else // For Unix and other
                    {
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
                    throw new Exception($"No directory in \"{projectDir}\"");
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