using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(VerticalGroupAttribute))]
    public class VerticalGroupDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var verticalGroup = attribute as VerticalGroupAttribute;
			var verticalGroupStyle = verticalGroup.DrawInBox ? EditorStyles.helpBox : EditorStyles.inspectorFullWidthMargins;

			EditorGUILayout.BeginVertical(verticalGroupStyle);

			EditorGUIUtility.labelWidth = verticalGroup.LabelWidth;
			EditorGUIUtility.fieldWidth = verticalGroup.FieldWidth;
		
			foreach (string variableName in verticalGroup.FieldsToGroup)
			{
				var variableProperty = FindNestedProperty(property, variableName);

				// Check for serialized properties since they have a weird naming when serialized and they cannot be found by the normal name
				variableProperty ??= FindNestedProperty(property, $"<{variableName}>k__BackingField");

				if (variableProperty != null)
				{
					EditorGUILayout.PropertyField(variableProperty, true);
				}
				else
				{
					EditorGUILayout.HelpBox($"{variableName} is not a valid field", MessageType.Error);
					break;
				}
			}

			EditorGUILayout.EndVertical();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
	}
}
