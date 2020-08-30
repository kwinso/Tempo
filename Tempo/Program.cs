using System;

namespace Tempo
{
    static class Program
    {
        static void Main(string[] args)
        {
            var creator = new Creator();

            try
            {
                SettingsManager.Load();
                
                var templateParsed = TryParseTemplateInfo(args, out var groupInfo);
            
                if (templateParsed || groupInfo != null) // Template options parsed
                {
                    creator.NewProject(groupInfo);
                    return;
                }
                
                /* Parsing another commands */
                if (args.Length > 0)
                {
                    if (args.Length == 1 && args[0] == "list")
                    {
                        SettingsManager.ListGroups();
                        return;
                    }
                    
                    if (args[0] == "add")
                    {
                        var group = TryParseNewGroupInfo(args);
                        if (group != null)
                        {
                            SettingsManager.AddNewTemplateGroup(group);
                            return;
                        }
                
                        Logger.Default(Help.AddMessage);
                        return;
                    }
                    
                    if (args.Length == 2 && args[0] == "remove")
                    {
                        SettingsManager.RemoveTemplateGroup(args[1]); // Removing by name
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

        // Try to parse syntax like "add language:name /path/to/template"
        private static TemplateGroup TryParseNewGroupInfo(string[] args)
        {
            if (args.Length != 3)
            {
                return null;
            }

            // Parsing string from "language:name" to ["language", "name"]
            var templateArgs = args[1].Split(":");

            if (templateArgs.Length != 2)
            {
                return null;
            }
            
            return new TemplateGroup(templateArgs[0], templateArgs[1], args[2]);
        }
        
        private static bool TryParseTemplateInfo(string[] args, out TemplateInfo templateInfo)
        {
            if (args.Length < 2)
            {
                templateInfo = null;
                return false;
            }

            // Command is a string like "language:template", it parses to ["language", "template"]
            var command = args[0].Split(":");
            if (command.Length == 2)
            {
                if (String.IsNullOrEmpty(command[0]))
                {
                    throw new NullReferenceException("Template group name cannot be skipped.");
                }

                if (String.IsNullOrEmpty(command[1]))
                {
                    throw new NullReferenceException("Template name cannot be skipped.");
                }
            }
            else
            {
                templateInfo = null;
                return false;
            }
            
            templateInfo = new TemplateInfo()
            {
                GroupName = command[0],
                Template = command[1],
                ProjectName = args[1]
            };
            
            return true;
        }
    }
}