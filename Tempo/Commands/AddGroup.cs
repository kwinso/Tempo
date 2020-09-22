using System;

namespace Tempo.Commands
{
    public class AddGroup : Command
    {
        private TemplateGroup TryParseNewGroupInfo(string[] args)
        {
            
            // Parsing string from "language:name" to ["language", "name"]
            var templateArgs = args[1].Split(":");
            

            if (templateArgs.Length > 2)
            {
                return null;
            }

            if (templateArgs.Length == 1)
            {
                return new TemplateGroup(null, templateArgs[0], args[2]);
            }
            return new TemplateGroup(templateArgs[1], templateArgs[0], args[2]);
        }

        public override bool Match(string[] args)
        {
            return args.Length >= 3 && args[0] == "add"; 
        }

        public override void Execute(string[] args)
        {
            var group = TryParseNewGroupInfo(args);
            if (group == null)
            {
                throw new ArgumentException("Invalid syntax for adding group. Try: tempo help add");
            }
            SettingsManager.AddNewTemplateGroup(group);
        }
    }
}