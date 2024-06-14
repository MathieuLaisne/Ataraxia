using UnityEngine;
using UnityEditor;
using Exion.Ataraxia.Default;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.ComponentModel;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

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

            Rect rectFoldout = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = true;

            int lines = 1;

            if (property.isExpanded)
            {
                var nameRect = new Rect(position.min.x, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 2, EditorGUIUtility.singleLineHeight);
                var statusRect = new Rect(position.min.x + position.max.x / 2 + 10, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 4, EditorGUIUtility.singleLineHeight);
                var amountRect = new Rect(position.max.x - position.max.x / 12 - 10, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 12, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
                if ((ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_BUILDING || (ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_CHARACTER || (ModifierType)property.FindPropertyRelative("Name").enumValueIndex == ModifierType.APPLY_EFFECT_TO_ALL_IN_BUILDING)
                {
                    EditorGUI.PropertyField(statusRect, property.FindPropertyRelative("statusType"), GUIContent.none);
                }
                EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("Amount"), GUIContent.none);

                lines++;
                var targetRect = new Rect(position.min.x, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 2, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(targetRect, property.FindPropertyRelative("target"), GUIContent.none);

                lines++;
                var requirementField = new Rect(position.max.x + 50, position.min.y + lines * EditorGUIUtility.singleLineHeight, 0, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(requirementField, property.FindPropertyRelative("RequirementType"));
                property.FindPropertyRelative("RequirementType").isExpanded = true;

                lines++;
                SerializedProperty arrayProp = property.FindPropertyRelative("RequirementType");
                if (arrayProp.isExpanded)
                {
                    for (int i = 0; i < arrayProp.arraySize; i++)
                    {
                        var value = arrayProp.GetArrayElementAtIndex(i);

                        requirementField = new Rect(position.min.x, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 1.5f, EditorGUIUtility.singleLineHeight);
                        EditorGUI.PropertyField(requirementField, value);

                        if ((RequirementType)value.enumValueIndex == RequirementType.AMOUNT_OF_STATUS_IS)
                        {
                            var amountRequiredField = new Rect(position.min.x + position.max.x / 1.5f + 10, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 6, EditorGUIUtility.singleLineHeight);
                            EditorGUI.PropertyField(amountRequiredField, property.FindPropertyRelative("AmountRequired"), GUIContent.none);
                        }
                        if ((RequirementType)value.enumValueIndex == RequirementType.TIME_IS)
                        {
                            var timePeriodRequiredField = new Rect(position.min.x + position.max.x / 1.5f + 10, position.min.y + lines * EditorGUIUtility.singleLineHeight, position.max.x / 6, EditorGUIUtility.singleLineHeight);
                            EditorGUI.PropertyField(timePeriodRequiredField, property.FindPropertyRelative("timePeriod"), GUIContent.none);
                        }
                        lines++;
                    }
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 4;

            if (property.FindPropertyRelative("RequirementType").isExpanded)
            {
                totalLines += property.FindPropertyRelative("RequirementType").CountInProperty();
            }

            return EditorGUIUtility.singleLineHeight * totalLines + EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}