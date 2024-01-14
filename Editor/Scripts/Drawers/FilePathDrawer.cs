using System.IO;
using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathDrawer : PropertyDrawerBase
    {
		private string filePath;

    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
			var filePathAttribute = attribute as FilePathAttribute;

			if (property.propertyType != SerializedPropertyType.String)
			{
				EditorGUILayout.HelpBox("The FilePath Attribute can only be attached to a string", MessageType.Error);
				return;
			}

			filePath = EditorPrefs.GetString($"{property.serializedObject.targetObject}_{property.name}_FilePath");

			EditorGUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();

			property.stringValue = EditorGUILayout.TextField(label, filePath);

			var buttonIcon = EditorGUIUtility.IconContent("d_Folder Icon");

			if (GUILayout.Button(buttonIcon, GUILayout.Width(30f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
				filePath = EditorUtility.OpenFilePanel("Select file", "Assets", filePathAttribute.Filters);
			
			EditorGUILayout.EndHorizontal();

			if (EditorGUI.EndChangeCheck())
			{
				if (filePathAttribute.GetRelativePath && !string.IsNullOrEmpty(filePath) && Path.IsPathFullyQualified(filePath))
					filePath = Path.GetRelativePath("Assets", filePath);

				EditorPrefs.SetString($"{property.serializedObject.targetObject}_{property.name}_FilePath", filePath);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;
	}
}
