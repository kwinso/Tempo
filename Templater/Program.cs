namespace Templater
{
    static class Program
    {
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);
            if (arguments == null)
            {
                Logger.Default("Usage: templater <language>:<template> <project_name>");
                Logger.Default("For example: templater node:socket mySocketApp -> Creates NodeJS socket.io server.");
                return;
            }

            var creator = new Creator();
            
            creator.NewProject(arguments);
        }
        
        private static Arguments ParseArguments(string[] args)
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