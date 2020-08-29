using System.Collections.Generic;

namespace Templater
{
    public sealed class Settings
    {
        public List<Template> Templates { get; set; }
    }

    public sealed class Template
    {
        public string Language { get; set; }
        public string Path { get; set; }
    }
}