using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public static class ConfigurationManager
{
    private const string CONFIG_EXTENSION = ".config";

    private static Dictionary<Type, object> _cachedObjects = new Dictionary<Type, object>();

    public static T Load<T>(string directory)
    {
        return (T)Load(typeof(T), directory);
    }
    private static object Load(Type type, string directory)
    {
        if (_cachedObjects.ContainsKey(type))
            return _cachedObjects[type];

        return LoadConfigFile(type, directory);
    }
    private static object CreateNewConfiguration(Type type, string directory)
    {
        Output.Line($"Creating new configuration for {type.Name}");

        XmlSerializer serializer = new XmlSerializer(type);
        object obj = Activator.CreateInstance(type);

        using (FileStream stream = new FileStream($"{directory}/{type.Name}{CONFIG_EXTENSION}", FileMode.Create))
        {
            serializer.Serialize(stream, obj);
        }

        return obj;
    }
    private static object LoadConfigFile(Type type, string directory)
    {
        string fileName = type.Name;
        string fullPath = $"{directory}/{fileName}{CONFIG_EXTENSION}";

        Output.DebugLine($"Loading {fileName}");

        if (File.Exists(fullPath))
        {
            XmlDocument document = new XmlDocument();
            document.Load(fullPath);

            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(new XmlNodeReader(document));
        }
        else
        {
            return CreateNewConfiguration(type, directory);
        }
    }
}
