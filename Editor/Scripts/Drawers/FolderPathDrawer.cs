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

            folderPath = EditorPrefs.GetString($"{property.serializedObject.targetObject}_{property.name}_FolderPath");

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();

			property.stringValue = EditorGUILayout.TextField(label, folderPath);

			var buttonIcon = EditorGUIUtility.IconContent("d_Folder Icon");

			if (GUILayout.Button(buttonIcon, GUILayout.Width(30f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
				folderPath = EditorUtility.OpenFolderPanel("Select folder", "Assets", "");

			EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                if (folderPathAttribute.GetRelativePath && !string.IsNullOrEmpty(folderPath))
					folderPath = Path.GetRelativePath("Assets", folderPath);

                EditorPrefs.SetString($"{property.serializedObject.targetObject}_{property.name}_FolderPath", folderPath);
			}
    	}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;
	}
}
