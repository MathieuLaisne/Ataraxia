using UnityEngine;
using UnityEditor;
using Exion.Ataraxia.Default;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Exion.Ataraxia.MyEditor
{
    [CustomPropertyDrawer(typeof(Modifier))]
    public class ModifierEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var nameRect = new Rect(position.x, position.y, 150, position.height);
            var statusRect = new Rect(position.x + 160, position.y, 50, position.height);
            var amountRect = new Rect(position.x + 220, position.y, 30, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
            if((ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_BUILDING || (ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_CHARACTER || (ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_ALL_IN_BUILDING)
            {
                EditorGUI.PropertyField(statusRect, property.FindPropertyRelative("statusType"), GUIContent.none);
            }
            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Amount"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}