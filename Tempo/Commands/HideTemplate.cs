using System;
using System.Linq;

namespace Tempo.Commands
{
    public class HideTemplate : Command
    {
        public override bool Match(string[] args)
        {
            return args.Length >= 2 && args[0] == "hide";
        }

        public override void Execute(string[] args)
        {
            if (args.ElementAtOrDefault(1) == null)
            {
                throw new ArgumentException("Required params were not set. Not set: template name to hide.");
            }
            var names = HelperFunctions.GetGroupAndTemplateNames(args[1]);

            if (names == null)
            {
                throw new ArgumentException("Invalid syntax for template. See: tempo help hide");
            }

            if (!SettingsManager.IsLoaded)
            {
                throw new NullReferenceException("Cannot access the Settings File. Is it in your installation folder?");
            }
            
            SettingsManager.HideTemplate(names[0], names[1]);
        }
    }
}