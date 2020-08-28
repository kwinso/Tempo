using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Templater
{
    public static class CommandLine
    {
        public static void RunCommand(string commandToRun, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "/usr/bin/bash",
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = commandToRun + " && exit",
                WorkingDirectory = workingDirectory

            };


            var process = Process.Start(processStartInfo);

            if (process == null)
            {
                throw new Exception("Process should not be null.");
            }
            
            process.WaitForExit();
        }
    }
}