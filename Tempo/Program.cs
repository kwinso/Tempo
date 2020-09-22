using System;
using System.Collections.Generic;
using Tempo.Commands;

namespace Tempo
{
    static class Program
    {
        static void Main(string[] args)
        {
            SettingsManager.Load();
            
            var commands = new List<Command>()
            {
                new CreateTemplate(),
                new ListTemplates(),
                new AddGroup(),
                new RemoveTemplate(),
                new HideTemplate(),
                new ShowTemplate()
            };

            try
            {
                foreach (var command in commands)
                {
                    if (command.Match(args))
                    {
                        command.Execute(args);
                        return;
                    }
                }
                
                Help.ShowFullHelp();
                Logger.Default("Press <Enter> to exit...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}