using System;
using System.Linq;

namespace Tempo.Commands
{
    public class ShowTemplate : Command
    {
        public override bool Match(string[] args)
        {
            return args.Length >= 2 && args[0] == "show";
        }

        public override void Execute(string[] args)
        {
            if (args.ElementAtOrDefault(1) == null)
            {
                throw new Exception("Required params were not set.\nNot set: template name to remove from hidden.");
            }
            var names = HelperFunctions.GetGroupAndTemplateNames(args[1]);

            if (names == null)
            {
                throw new ArgumentException("Template name. Try: tempo help show");
            }

            if (!SettingsManager.IsLoaded)
            {
                throw new NullReferenceException("Cannot access the Settings File. Is it in your installation folder?");
            }
            
            SettingsManager.RemoveFromHidden(names[0], names[1]);
        }
    }
}