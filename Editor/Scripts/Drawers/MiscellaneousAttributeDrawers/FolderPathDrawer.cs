using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var folderPathAttribute = attribute as FolderPathAttribute;
			var root = new VisualElement();

			if (property.propertyType != SerializedPropertyType.String)
			{
				root.Add(new HelpBox("The FolderPath Attribute can only be attached to a string", HelpBoxMessageType.Error));
				return root;
			}

			var folderPath = property.stringValue;

			var propertyField = DrawProperty(property);
			var button = new Button(() =>
			{
				folderPath = EditorUtility.OpenFolderPanel("Select folder", "Assets", "");
			});

			var buttonIcon = new Image() { image = EditorGUIUtility.IconContent("d_Folder Icon").image };

			UpdateVisualElement(root, () =>
			{
				if (folderPathAttribute.GetRelativePath && !string.IsNullOrEmpty(folderPath))
				{
					string projectRoot = Application.dataPath[..^"Assets".Length];

					folderPath = Path.GetRelativePath(projectRoot, folderPath);
				}

				if (property.hasMultipleDifferentValues)
					return;

				property.stringValue = folderPath;
				propertyField.Q<TextField>().value = folderPath;
				property.serializedObject.ApplyModifiedProperties();
			});

			button.style.width = 40f;
			button.style.height = 20f;
			propertyField.style.flexGrow = 1f;
			root.style.flexDirection = FlexDirection.Row;

			button.Add(buttonIcon);
			root.Add(propertyField);
			root.Add(button);

			return root;
		}
	}
}
