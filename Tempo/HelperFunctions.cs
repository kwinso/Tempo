using System;
using System.Text.RegularExpressions;

namespace Tempo
{
    public static class HelperFunctions
    {
        public static string[] GetGroupAndTemplateNames(string str) 
        {
            // Command is a string like "language/template", it parses to ["language", "template"]
            
            var regex = new Regex(Regex.Escape("/"));
            
            var names = regex.Replace(str, ":", 1).Split(":");

            // Checking for invalid data
            if (names.Length == 2)
            {
                if (String.IsNullOrEmpty(names[0]))
                {
                    throw new NullReferenceException("Template group name cannot be skipped.");
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
            return names;
        }
    }
}