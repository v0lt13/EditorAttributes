using System.IO;
using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawerBase
    {
        private string folderPath;

    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var folderPathAttribute = attribute as FolderPathAttribute;

            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUILayout.HelpBox("The FolderPath Attribute can only be attached to a string", MessageType.Error);
                return;
            }

            folderPath = EditorPrefs.GetString($"{property.serializedObject.targetObject}_{property.propertyPath}_FolderPath");

            EditorGUI.BeginChangeCheck();

            float buttonWidth = 30f;
            var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth, position.height);

			property.stringValue = EditorGUI.TextField(fieldRect, label, folderPath);

			var buttonIcon = EditorGUIUtility.IconContent("d_Folder Icon");
            var buttonRect = new Rect(fieldRect.xMax + 2f, position.y, buttonWidth, position.height);

			if (GUI.Button(buttonRect, buttonIcon))
				folderPath = EditorUtility.OpenFolderPanel("Select folder", "Assets", "");

            if (EditorGUI.EndChangeCheck())
            {
                if (folderPathAttribute.GetRelativePath && !string.IsNullOrEmpty(folderPath))
                {
					string projectRoot = Application.dataPath[..^"Assets".Length];

					folderPath = Path.GetRelativePath(projectRoot, folderPath);
                }

                EditorPrefs.SetString($"{property.serializedObject.targetObject}_{property.propertyPath}_FolderPath", folderPath);
			}
    	}
	}
}
