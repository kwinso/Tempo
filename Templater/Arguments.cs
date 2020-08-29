namespace Templater
{
    public class Arguments
    {
        public string Language { get; set; } // Language of project, e.g. "node" for nodejs
        public string Template { get; set; } // Specific type of project
        
        public string ProjectName { get; set; }
        
    }
}