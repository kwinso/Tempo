using System;
using System.IO;
using System.Linq;

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
                    switch (args[0])
                    {
                        case "list":
                        {
                            SettingsManager.ListGroups();
                            return;
                        }
                        case "remove":
                        case "hide":
                        case "show":
                        {
                            if (args[0] == "remove")
                            {
                                if (args.ElementAtOrDefault(1) == null)
                                {
                                    Logger.Default(Help.RemoveMessage);
                                    return;
                                }

                                var groupName = args[1].Substring(1);
                                SettingsManager.RemoveTemplateGroup(groupName); // Removing by name
                                return;
                            }


                            string[] names = null;
                            if (args.ElementAtOrDefault(1) != null)
                            {
                                names = GetGroupAndTemplateNames(args[1]);
                            }

                            // Ignoring template
                            if (args[0] == "hide")
                            {
                                if (names == null)
                                {
                                    Logger.Default(Help.UnwatchMessage);
                                    return;
                                }

                                SettingsManager.HideTemplate(names[0], names[1]);
                                return;
                            }

                            if (args[0] == "show")
                            {
                                if (names == null)
                                {
                                    Logger.Default(Help.WatchMessage);
                                    return;
                                }

                                SettingsManager.RemoveFromHidden(names[0], names[1]);

                            }

                            return;
                        }
                        case "add":
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

        // Try to parse syntax like "add name:language /path/to/template"
        private static TemplateGroup TryParseNewGroupInfo(string[] args)
        {
            if (args.Length != 3)
            {
                return null;
            }
            
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

        private static string[] GetGroupAndTemplateNames(string str) 
        {
            // Command is a string like "@language/template", it parses to ["@language", "template"]
            var names = str.Split("/");
            
            // Checking for invalid data
            if (names.Length == 2)
            {
                if (String.IsNullOrEmpty(names[0]))
                {
                    throw new NullReferenceException("Template group name cannot be skipped.");
                }

                if (!names[0].StartsWith("@"))
                {
                    throw  new ArgumentException($"Group name must be identified with \"@\".\nTry @{names[0]}");
                }

                if (String.IsNullOrEmpty(names[1]))
                {
                    throw new NullReferenceException("Template name cannot be skipped.");
                }
            }
            else
            {
                return null;
            }
            
            // Removing @ sign
            names[0] = names[0].Substring(1);
            return names;
        }
        
        private static bool TryParseTemplateInfo(string[] args, out TemplateInfo templateInfo)
        {
            if (args.Length < 2)
            {
                templateInfo = null;
                return false;
            }

            var names = GetGroupAndTemplateNames(args[0]);
            
            if (names == null) 
            {
                templateInfo = null;
                return false;
            }

            templateInfo = new TemplateInfo()
            {
                GroupName = names[0],
                Template = names[1],
                ProjectName = args[1]
            };
            
            return true;
        }
    }
}