namespace Templater
{
    static class Program
    {
        static void Main(string[] args)
        {
            var creator = new Creator();

            var settingsLoaded = creator.LoadSettings();
            
            if (!settingsLoaded)
            {
                Logger.Error("No settings file found.");
                return;
            }
            
            var arguments = ParseTemplate(args);
            
            if (arguments == null)
            {
                if (args.Length == 1 && args[0] == "list")
                {
                    creator.ShowTemplates();
                    return;
                }
                
                Logger.Default("Usage: templater <language>:<template> <project_name>");
                Logger.Default("For example: templater node:socket mySocketApp -> Creates NodeJS socket.io server.");
                Logger.Default("Use: templater list for see available templates.");
                return;
            }
            
            creator.NewProject(arguments);
        }
        
        private static Arguments ParseTemplate(string[] args)
        {
            if (args.Length < 2)
            {
                return null;
            }

            // Command is a string like "language:template", it parses to ["language", "template"]
            var command = args[0].Split(":");
            
            if (command.Length != 2)
            {
                return null;
            }

            return new Arguments()
            {
                Language = command[0],
                Template = command[1],
                ProjectName = args[1]
            };
        }
    }
}