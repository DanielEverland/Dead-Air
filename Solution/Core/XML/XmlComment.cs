using System;

namespace XML
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class XmlComment : Attribute
    {
        public XmlComment(string text)
        {
            Text = text;
        }

        public string Text;
    }
}