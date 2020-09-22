namespace Tempo
{
    public static class Help
    {
        public static string UnwatchMessage =>
            "Command \"show\":\n" +
            "\t Adds name of a template to unwatched in group.\n" +
            "\t Syntax: tempo show @<template_group_name>/<name_to_be_ignored>\n";
        
        public static string ShowTemplateMessage =>
            "Command \"hide\":\n" +
            "\t Removes name from unwatched in group.\n" +
            "\t Syntax: tempo hide @<template_group_name>/<name_to_be_watched>\n";
        
        public static string ListMessage =>
            "Command \"list\":\n" +
            "\t Lists all available templates and shows potential warnings.\n" +
            "\t Syntax: tempo list\n";

        public static string AddMessage =>
            "Command \"add\"\n" +
            "\t Adds new template folder to settings.json.\n" +
            "\t Syntax: tempo add <template_group_name>:<language> <path to the templates group folder>\n";
        
        public static string RemoveMessage =>
            "Command \"remove\"\n" +
            "\t Remove existing templates folder from settings.json by name.\n" +
            "\t Syntax: tempo remove @<template group name>\n" +
            "\t <template group name> - name of template group you want to remove from program scope.\n";

        public static string CreateFromTemplateMessage =>
            "How to use tempo:\n" +
            "\t Syntax: tempo @<template_group_name>/<template> <project_name>\n" +
            "\t Where:\n" +
            "\t @<template_group_name> - Name of Template group of your template.\n" +
            "\t <template> - Name of a template(folder) where your template is stored.\n" +
            "\t <project_name> - Name of your project folder (without whitespaces).\n";

        public static void ShowFullHelp()
        {
            Logger.Default(
                $"{CreateFromTemplateMessage}\n" +
                $"{ListMessage}\n" +
                $"{AddMessage}\n" +
                $"{RemoveMessage}\n" +
                $"{UnwatchMessage}\n" +
                $"{ShowTemplateMessage}\n"
            );
            Logger.Info("Also there's a documentation in the Repository: https://github.com/uwumouse/Tempo");
        }
    }
}