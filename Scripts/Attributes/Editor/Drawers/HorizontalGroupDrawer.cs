using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalGroupAttribute))]
    public class HorizontalGroupDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var horizontalGroup = attribute as HorizontalGroupAttribute;
			var serializedObject = property.serializedObject;

			var horizontalGroupStyle = horizontalGroup.DrawInBox ? EditorStyles.helpBox : EditorStyles.inspectorFullWidthMargins;

			EditorGUILayout.BeginHorizontal(horizontalGroupStyle);
		
			EditorGUIUtility.labelWidth = horizontalGroup.LabelWidth;
			EditorGUIUtility.fieldWidth = horizontalGroup.FieldWidth;
		
			foreach (string variableName in horizontalGroup.FieldsToGroup)
			{
				var variableProperty = serializedObject.FindProperty(variableName);

				if (variableProperty.type == "Void")
				{
					EditorGUI.PropertyField(position, variableProperty, true);
				}
				else if (variableProperty != null)
				{
					EditorGUILayout.PropertyField(variableProperty, true);
				}
				else
				{
					EditorGUILayout.HelpBox($"{variableName} is not a valid field", MessageType.Error);
					break;
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the holder field
	}
}
