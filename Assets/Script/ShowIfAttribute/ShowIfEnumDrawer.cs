using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute condition = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProp = property.serializedObject.FindProperty(condition.enumFieldName);

        if (enumProp != null && enumProp.propertyType == SerializedPropertyType.Enum)
        {
            if (enumProp.enumNames[enumProp.enumValueIndex] == condition.targetValue.ToString())
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            Debug.LogWarning($"[ShowIfEnum] 找不到 enum 欄位: {condition.enumFieldName}");
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute condition = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProp = property.serializedObject.FindProperty(condition.enumFieldName);

        if (enumProp != null && enumProp.propertyType == SerializedPropertyType.Enum)
        {
            if (enumProp.enumNames[enumProp.enumValueIndex] == condition.targetValue.ToString())
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }
            else
            {
                return 0f;
            }
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
