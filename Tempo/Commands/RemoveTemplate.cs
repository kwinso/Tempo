using System;
using System.Linq;

namespace Tempo.Commands
{
    public class RemoveTemplate : Command
    {
        public override bool Match(string[] args)
        {
            return args.Length >= 2 && args[0] == "remove";
        }

        public override void Execute(string[] args)
        {
            if (args.ElementAtOrDefault(1) == null)
            {
                throw new ArgumentException("Required params were not set. Not set: templates group name to remove.");
            }
            
            SettingsManager.RemoveTemplateGroup(args[1]); // Removing by name
        }
    }
}