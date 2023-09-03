using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalGroupAttribute))]
    public class HorizontalGroupDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var horizontalGroup = attribute as HorizontalGroupAttribute;
			var serializedObject = property.serializedObject;
		
			EditorGUILayout.BeginHorizontal();
		
			EditorGUIUtility.labelWidth = horizontalGroup.labelWidth;
			EditorGUIUtility.fieldWidth = horizontalGroup.fieldWidth;
		
			foreach (string variableName in horizontalGroup.fieldsToGroup)
			{
				var variableProperty = serializedObject.FindProperty(variableName);
				
				if (variableProperty != null) EditorGUILayout.PropertyField(variableProperty, true);
			}
		
			EditorGUILayout.EndHorizontal();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
	}
}
