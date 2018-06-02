using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class XmlComment : Attribute {

    public XmlComment(string text)
    {
        Text = text;
    }

    public string Text;
}
