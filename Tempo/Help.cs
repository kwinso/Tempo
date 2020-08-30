namespace Tempo
{
    public static class Help
    {
        public static string ListMessage =>
            "Command \"list\":\n" +
            "\t Lists all available templates and shows potential warnings.\n" +
            "\t Syntax: tempo list\n";

        public static string AddMessage =>
            "Command \"add\"\n" +
            "\t Adds new template folder to settings.json.\n" +
            "\t Syntax: tempo add <language>:<template_group_name> <path to the templates group folder>\n";
        
        public static string RemoveMessage =>
            "Command \"remove\"\n" +
            "\t Remove existing templates folder from settings.json by name.\n" +
            "\t Syntax: tempo remove <template group name>\n" +
            "\t <template group name> - name of template group you want to remove from program scope.\n";

        public static string CreateFromTemplateMessage =>
            "How to use tempo:\n" +
            "\t Syntax: tempo <template_group_name>:<template> <project_name>\n" +
            "\t Where:\n" +
            "\t <template_group_name> - Name of Template group of your template.\n" +
            "\t <template> - Name of a template(folder) where your template is stored.\n" +
            "\t <project_name> - Name of your project folder (without whitespaces).\n";

        public static void ShowFullHelp()
        {
            Logger.Default($"{CreateFromTemplateMessage}\n{ListMessage}\n{AddMessage}\n{RemoveMessage}\n");
            Logger.Info("Also there's a documentation in the Repository: https://github.com/uwumouse/Tempo");
        }
    }
}