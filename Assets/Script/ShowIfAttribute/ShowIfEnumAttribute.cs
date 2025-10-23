using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfEnumAttribute : PropertyAttribute
{
    public string enumFieldName;
    public object targetValue;

    public ShowIfEnumAttribute(string enumFieldName, object targetValue)
    {
        this.enumFieldName = enumFieldName;
        this.targetValue = targetValue;
    }
}
