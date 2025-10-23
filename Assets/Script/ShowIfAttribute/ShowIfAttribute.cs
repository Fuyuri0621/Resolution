using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    public string conditionBool;

    public ShowIfAttribute(string conditionBool)
    {
        this.conditionBool = conditionBool;
    }
}
