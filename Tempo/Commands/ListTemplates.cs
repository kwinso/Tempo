using System;

namespace Tempo.Commands
{
    public class ListTemplates : Command
    {
        public override bool Match(string[] args)
        {
            return args.Length >= 1 && args[0] == "list";
        }

        public override void Execute(string[] args)
        {
            if (!SettingsManager.IsLoaded)
            {
                throw  new NullReferenceException("Templates cannot be listed because settings file is not loaded.");
            }
            
            SettingsManager.ListGroups();
        }
    }
}