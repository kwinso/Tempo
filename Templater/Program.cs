using System;

namespace Templater
{
    static class Program
    {
        static void Main(string[] args)
        {
            var arguments = ParseArguments(args);
            if (arguments == null)
            {
                Console.WriteLine("Usage: templater new <language> <template> <project_name>");
                Console.WriteLine("For example: templater new node socket mySockets -> Creates NodeJS socket.io server.");
                return;
            }
            Console.WriteLine(arguments.Language);

            var creator = new Creator();

            if (arguments.Mode == "new")
            {
                creator.NewProject(arguments);
            }
            else
            {
                Console.WriteLine($"Unknown command {arguments.Mode}");
            }
        }

        // TODO: Make another arguments system
        // Like this: templater language:template <name>
        private static Arguments ParseArguments(string[] args)
        {
            if (args.Length < 4)
            {
                return null;
            }

            return new Arguments()
            {
                Mode = args[0],
                Language = args[1],
                Template = args[2],
                ProjectName = args[3]
            };
        }
    }
}