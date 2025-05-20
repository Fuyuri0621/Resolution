using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class HideIfAttribute : PropertyAttribute
{
    public string conditionBool;

    public HideIfAttribute(string conditionBool)
    {
        this.conditionBool = conditionBool;
    }
}
