namespace Tempo
{
    public class TemplateInfo
    {
        public string GroupName { get; set; } // Language of project, e.g. "node" for nodejs
        
        public string Template { get; set; } // Specific type of project
        
        public string ProjectName { get; set; }
        
    }
}