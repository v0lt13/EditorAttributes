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

			filePath = EditorPrefs.GetString($"{property.serializedObject.targetObject}_{property.propertyPath}_FilePath");

			EditorGUI.BeginChangeCheck();

			float buttonWidth = 30f;
			var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth, position.height);

			property.stringValue = EditorGUI.TextField(fieldRect, label, filePath);

			var buttonIcon = EditorGUIUtility.IconContent("d_Folder Icon");
			var buttonRect = new Rect(fieldRect.xMax + 2f, position.y, buttonWidth, position.height);

			if (GUI.Button(buttonRect, buttonIcon))
				filePath = EditorUtility.OpenFilePanel("Select file", "Assets", filePathAttribute.Filters);

			if (EditorGUI.EndChangeCheck())
			{
				if (filePathAttribute.GetRelativePath && !string.IsNullOrEmpty(filePath) && Path.IsPathFullyQualified(filePath))
				{
					string projectRoot = Application.dataPath[..^"Assets".Length];

					filePath = Path.GetRelativePath(projectRoot, filePath);
				}

				EditorPrefs.SetString($"{property.serializedObject.targetObject}_{property.propertyPath}_FilePath", filePath);
			}
		}
	}
}
