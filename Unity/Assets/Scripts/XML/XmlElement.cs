using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class XmlElement : Attribute {

    public XmlElement(string name)
    {
        Name = name;
    }

    public string Name;
}
