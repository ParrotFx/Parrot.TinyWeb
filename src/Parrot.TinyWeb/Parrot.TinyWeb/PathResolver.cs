namespace Parrot.TinyWeb
{
    using System;
    using System.IO;
    using Parrot.Renderers.Infrastructure;

    public class PathResolver : IPathResolver
    {
        static string[] ViewPaths = new string[] {
            "Views/{0}.parrot",
            "{0}.parrot",
            "Views/Shared/{0}.parrot",
            "Shared/{0}.parrot",
            //These are for template names that include extension
            "Views/{0}",
            "{0}",
            "Views/Shared/{0}",
            "Shared/{0}",
        };

        public Stream OpenFile(string path)
        {
            return new FileStream(ResolveTemplatePath(path), FileMode.Open);
        }

        public bool FileExists(string path)
        {
            return ResolveTemplatePath(path) != null;
        }

        public string ResolvePath(string path)
        {
            string fullTemplatePath = ResolveTemplatePath(path);

            if (FileExists(fullTemplatePath))
            {
                //add it to cache - not currently implemented
                return fullTemplatePath;
            }

            throw new FileNotFoundException(path);
        }

        public static string ResolveTemplatePath(string templateName, string[] optionalSearchPaths = null)
        {
            var templatePath = AppDomain.CurrentDomain.BaseDirectory;

            string fullTemplatePath = null;

            foreach (var path in ViewPaths)
            {
                string fullPath = Path.Combine(templatePath, string.Format(path, templateName));

                if (File.Exists(fullPath))
                {
                    fullTemplatePath = fullPath;
                    break;
                }
            }

            if (fullTemplatePath == null && optionalSearchPaths != null)
            {
                foreach (var optionalPath in optionalSearchPaths)
                {
                    foreach (var path in ViewPaths)
                    {
                        string fullPath = Path.Combine(optionalPath, string.Format(path, templateName));

                        if (File.Exists(fullPath))
                        {
                            fullTemplatePath = fullPath;
                            break;
                        }
                    }
                }
            }

            return fullTemplatePath;
        }

        public string ResolveAttributeRelativePath(string key, object value)
        {
            //if (value != null)
            //{
            //    string temp = value.ToString();
            //    if (temp.StartsWith("~/") && !key.StartsWith("data-val", StringComparison.OrdinalIgnoreCase))
            //    {
            //        //convert this to a server path

            //        return UrlHelper.GenerateContentUrl(temp, new HttpContextWrapper(HttpContext.Current));
            //    }
            //    return temp;
            //}

            return null;
        }
    }
}