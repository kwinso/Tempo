using System;

namespace Tempo.Commands
{
    public class CreateTemplate : Command
    {
        private readonly Creator _creator = new Creator();
        
        public override bool Match(string[] args)
        {
            // Checking if first argument is a name of a template
            return args.Length >= 2 && args[0].Split("/").Length >= 2;
        }

        private void ParseArgs(string[] args, out TemplateInfo templateInfo)
        {
            if (args.Length < 2)
            {
                throw new ArgumentException("Invalid arguments. Type: tempo help create, to see how to use it.");
            }

            var names = HelperFunctions.GetGroupAndTemplateNames(args[0]);
            
            if (names == null) 
            {
                throw new ArgumentException("Invalid template name.");
            }

            templateInfo = new TemplateInfo()
            {
                GroupName = names[0],
                Template = names[1],
                ProjectName = args[1]
            };
        }

        public override void Execute(string[] args)
        {
            ParseArgs(args, out var templateInfo);
            
            _creator.NewProject(templateInfo);
        }
    }
}