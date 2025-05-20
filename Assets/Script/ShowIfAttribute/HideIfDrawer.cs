using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HideIfAttribute))]
public class HideIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideIfAttribute hideIf = (HideIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(hideIf.conditionBool);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            if (!conditionProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        else
        {
            Debug.LogWarning($"[HideIf] 沒有找到 bool 變數: {hideIf.conditionBool}");
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideIfAttribute hideIf = (HideIfAttribute)attribute;
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(hideIf.conditionBool);

        if (conditionProperty != null && conditionProperty.propertyType == SerializedPropertyType.Boolean)
        {
            return !conditionProperty.boolValue ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
        }

        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
