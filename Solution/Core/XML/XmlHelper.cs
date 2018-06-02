using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace XML
{
    public static class XmlHelper {

        private static BindingFlags _fieldBindings = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary>
        /// Adds linebreaks between elements with comments
        /// </summary>
        public static string PostProcessFormatting(string value)
        {
            StringWriter writer = new StringWriter();

            using (StringReader reader = new StringReader(value))
            {
                bool hasSeenComment = false;
                bool hasSeenElement = false;

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(IsComment(line))
                    {
                        hasSeenComment = true;
                    }
                    else if (IsElement(line))
                    {
                        hasSeenElement = true;
                    }

                    writer.WriteLine(line);

                    if (hasSeenElement && hasSeenComment && IsElement(line))
                    {
                        writer.WriteLine();
                        hasSeenComment = false;
                        hasSeenElement = false;
                    }
                }
            }

            return writer.ToString();
        }
        private static bool IsComment(string value)
        {
            return value.Trim(' ').StartsWith("<!");
        }
        private static bool IsElement(string value)
        {
            return value.Trim(' ').StartsWith("<") && !IsComment(value);
        }
        public static XDocument Serialize(object obj)
        {
            XDocument document = new XDocument();

            List<XNode> content = new List<XNode>();
            List<FieldInfo> fields = new List<FieldInfo>(obj.GetType().GetFields(_fieldBindings));

            foreach (FieldInfo field in fields)
            {
                XmlElement element = (XmlElement)field.GetCustomAttribute(typeof(XmlElement));

                if(element != null)
                {
                    foreach (XmlComment comment in field.GetCustomAttributes(typeof(XmlComment)))
                    {
                        content.Add(new XComment(comment.Text));
                    }

                    content.Add(new XElement(element.Name, field.GetValue(obj)));
                }
            }

            document.Add(new XElement(obj.GetType().Name, content));

            return document;
        }
        public static void Deserialize<T>(XDocument document, T instance)
        {
            List<FieldInfo> fields = new List<FieldInfo>(typeof(T).GetFields(_fieldBindings).Where(x => x.GetCustomAttributes(typeof(XmlElement)).Count() > 0));
            HashSet<string> usedNames = new HashSet<string>();

            foreach (XElement element in document.Descendants())
            {
                string name = element.Name.LocalName;
                        
                if (usedNames.Contains(name))
                    continue;

                AssignValue(name, element.Value, instance, fields);
                usedNames.Add(name);
            }
        }
        private static void AssignValue(string elementName, object value, object instance, List<FieldInfo> fields)
        {
            foreach (FieldInfo field in fields)
            {
                XmlElement element = (XmlElement)field.GetCustomAttribute(typeof(XmlElement));

                if (element.Name == elementName)
                {
                    field.SetValue(instance, Convert.ChangeType(value, field.FieldType));
                }
            }
        }
    }
}
