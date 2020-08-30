using System;

namespace Tempo
{
    public static class PathConverter
    {
        public static string ToAbsolutePath(string path)
        {
            if (path == null)
            {
                throw new NullReferenceException();
            }
            
            if (path.StartsWith("@local"))
            {
                path = path.Replace("@local",AppDomain.CurrentDomain.BaseDirectory);
            }

            return path;
        }
    }
}