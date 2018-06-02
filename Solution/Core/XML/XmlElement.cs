using System;

namespace XML
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class XmlElement : Attribute {

        public XmlElement(string name)
        {
            Name = name;
        }

        public string Name;
    }
}
