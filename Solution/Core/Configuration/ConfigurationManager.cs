using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using XML;

namespace Configuration
{
    public static class ConfigurationManager
    {
        private const string CONFIG_EXTENSION = ".config";

        private static Dictionary<Type, object> _cachedObjects = new Dictionary<Type, object>();

        public static T Load<T>(string directory)
        {
            Type type = typeof(T);

            if (_cachedObjects.ContainsKey(type))
            {
                return (T)_cachedObjects[type];
            }
            else
            {
                return LoadConfigFile<T>(directory);
            }
        }
        private static T LoadConfigFile<T>(string directory)
        {
            Type type = typeof(T);
            string fileName = type.Name;
            string fullPath = $"{directory}/{fileName}{CONFIG_EXTENSION}";

            Output.DebugLine($"Loading {fileName}");

            if (File.Exists(fullPath))
            {
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    XDocument document = XDocument.Load(stream);
                    T instance = Activator.CreateInstance<T>();

                    XmlHelper.Deserialize(document, instance);

                    return instance;
                }
            }
            else
            {
                return CreateNewConfiguration<T>(directory);
            }
        }
        private static T CreateNewConfiguration<T>(string directory)
        {
            Type type = typeof(T);
            Output.Line($"Creating new configuration for {type.Name}");

            Directories.EnsurePathExists(directory);

            object obj = Activator.CreateInstance<T>();
            XDocument document = XmlHelper.Serialize(obj);

            string documentText = document.ToString();
            documentText = XmlHelper.PostProcessFormatting(documentText);

            File.WriteAllText($"{directory}/{type.Name}{CONFIG_EXTENSION}", documentText);

            return (T)obj;
        }
    }
}
