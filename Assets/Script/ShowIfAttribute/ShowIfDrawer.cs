using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.conditionBool);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            if (conditionProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            Debug.LogWarning($"[ShowIf] 沒有找到 bool 變數: {showIf.conditionBool}");
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(showIf.conditionBool);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            return conditionProperty.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
