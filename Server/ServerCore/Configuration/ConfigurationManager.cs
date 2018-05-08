using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ServerCore.Configuration
{
    public static class ConfigurationManager
    {
        private static string ExecutableDirectory { get { return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location); } }

        private const string CONFIG_EXTENSION = ".config";

        private static Dictionary<System.Type, object> _cachedObjects = new Dictionary<System.Type, object>();

        public static T Load<T>()
        {
            return (T)Load(typeof(T));
        }
        private static object Load(Type type)
        {
            if (_cachedObjects.ContainsKey(type))
                return _cachedObjects[type];

            return LoadConfigFile(type);
        }
        private static object CreateNewConfiguration(System.Type type)
        {
            Output.Line($"Creating new configuration for {type.Name}");

            XmlSerializer serializer = new XmlSerializer(type);
            object obj = System.Activator.CreateInstance(type);

            using (FileStream stream = new FileStream(ExecutableDirectory + "/" + type.Name + CONFIG_EXTENSION, FileMode.Create))
            {
                serializer.Serialize(stream, obj);
            }

            return obj;
        }
        private static object LoadConfigFile(Type type)
        {
            string fileName = type.Name;
            string fullPath = ExecutableDirectory + "/" + fileName + CONFIG_EXTENSION;

            Output.DebugLine($"Loading {fileName}");
            
            if(File.Exists(fullPath))
            {
                XmlDocument document = new XmlDocument();
                document.Load(fullPath);

                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(new XmlNodeReader(document));
            }
            else
            {
                return CreateNewConfiguration(type);
            }
        }
    }
}
