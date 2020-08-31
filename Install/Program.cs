using System;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

namespace Installer
{
    /* This program just builds and configures Tempo */
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Start in Installer directory.
                Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

                DirectoryInfo outDir = new DirectoryInfo("../Tempo_Build"); // Build project one level up from installer

                if (IsOnWindows()) // Run this command only as administrator.
                {
                    if (!IsAdministrator())
                    {
                        Console.WriteLine("Please, run installer as Administrator.");
                        return;
                    }
                }

                if (outDir.Exists)
                {
                    Console.WriteLine($"Directory {outDir.FullName} already exists, override?");
                    Console.WriteLine("Stop the program and press <Enter> to proceed.");
                    Console.ReadLine();
                }

                outDir.Create();
                outDir.CreateSubdirectory("bin"); // Directory for shell scripts

                // Build Tempo
                var buildProcess = Process.Start("dotnet", $"build ../Tempo/Tempo.csproj -o {outDir.FullName}");
                if (buildProcess == null)
                {
                    Console.WriteLine("Dotnet is not installed.");
                    return;
                }

                buildProcess.WaitForExit();

                /* Copying default files if they do not exists */
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                if (!Directory.Exists($"{outDir.FullName}/Templates"))
                {
                    CopyDirectory(baseDir + "/Templates", $"{outDir.FullName}/Templates");
                }

                if (!File.Exists($"{outDir.FullName}settings.json"))
                {
                    File.Copy(baseDir + "/settings.json", $"{outDir.FullName}/settings.json");
                }
                
                
                Console.WriteLine("Creating program shell script.");

                CreateShellScript(outDir.FullName);

                Console.Write($"Tempo installed in {outDir.FullName}.\nPress <Enter> to exit...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to install Tempo:");
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        private static void CreateShellScript(string outDirPath)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var scriptPath = $"{outDirPath}/bin/tempo";
                var script = "$(dirname \"$0\")/../Tempo $@"; // Script to run Tempo executable 

                File.WriteAllText(scriptPath, script);
                var bash = Process.Start("chmod", "+x " + scriptPath);
                bash.WaitForExit();

                var binDirectory = new DirectoryInfo($"{outDirPath}/bin");

                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine(
                    $"Script created. If you want Tempo to work globally, add {binDirectory.FullName} to the PATH variable.");

                Console.ResetColor();
            }
            else
            {
                var script = "@echo off\n %~dp0../Tempo.exe %*"; // Script to run Tempo executable 
                File.WriteAllText($"{outDirPath}/bin/tempo.bat", script);

                var binDirectory = new DirectoryInfo($"{outDirPath}/bin");

                // Add .bat script file to PATH
                var path = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);
                Environment.SetEnvironmentVariable("PATH", $"{path}:{binDirectory.FullName}",
                    EnvironmentVariableTarget.Machine);

                Console.WriteLine("Script added to path.");
            }
        }


        private static bool IsOnWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        private static bool IsAdministrator()
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Copy directory recursively
        private static void CopyDirectory(string sourcePath, string destinationPath)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourcePath);

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
                CopyDirectory(subdir.FullName, temppath);
            }
        }
    }
}