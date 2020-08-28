using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Templater
{
    public class Creator
    {
        public void NewProject(Arguments args)
        {
            if (args.Language == "node")
            {
                CreateNodeProject(args.Template, args.ProjectName);
            }
            else
            {
                Console.WriteLine($"Unknown language {args.Language}");
            }
        }

        private void CreateNodeProject(string template, string projectName)
        {

            var projectRootDirectory = GetProjectDirectory();
            var projectPath = projectRootDirectory.FullName + "/" + projectName;
                
            projectRootDirectory.CreateSubdirectory(projectName);

            var projectDirectory = new DirectoryInfo(projectPath);
            if (template == "express")
            {
                Console.WriteLine("Creating new Express.js project");

                CopyFromTemplate(
                    $"{AppDomain.CurrentDomain.BaseDirectory}/Templates/Node/Express",
                    $"{projectDirectory.FullName}"
                );
            }
            else if (template == "socket")
            {
                Console.WriteLine($"Creating new Socket.io project");

                CopyFromTemplate(
                    $"{AppDomain.CurrentDomain.BaseDirectory}/Templates/Node/Socket",
                    $"{projectDirectory.FullName}"
                );
            }
            else if (template == "api")
            {
                Console.WriteLine($"Creating new ExpressJS API project");

                CopyFromTemplate(
                    $"{AppDomain.CurrentDomain.BaseDirectory}/Templates/Node/Api",
                    $"{projectDirectory.FullName}"
                );
            }
            else
            {
                Directory.Delete(projectDirectory.FullName);
                Console.WriteLine($"Unknown template \"{template}\"");
                return;
            }

            var packageFilePath = $"{projectDirectory.FullName}/package.json";
            
            var info = File.ReadAllText(packageFilePath);
            
            // Replacing default project name with user's value
            File.WriteAllText(packageFilePath,info.Replace("_project_", projectName));

            Directory.SetCurrentDirectory(projectDirectory.FullName);
            try
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Installing Dependencies...");
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
                Console.WriteLine("npm is not installed, creating is aborted.");
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
                    Console.WriteLine("Creating project in current directory");
                    return new DirectoryInfo(Directory.GetCurrentDirectory());
                }

                if (!Directory.Exists(projectDir.Trim()))
                {
                    Console.WriteLine($"No directory in \"{projectDir}\"");
                    continue;
                }
                
                return  new DirectoryInfo(projectDir);
            }
        }
        
        private static void CopyFromTemplate(string templatePath, string destinationPath)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(templatePath);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + templatePath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(temppath, true);
            }

            // Copy subdirectories
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destinationPath, subdir.Name);
                CopyFromTemplate(subdir.FullName, temppath);
            }
        }
    }
}