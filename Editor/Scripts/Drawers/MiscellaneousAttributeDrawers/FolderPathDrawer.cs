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

			var propertyField = CreatePropertyField(property);
			var button = new Button(() => folderPath = EditorUtility.OpenFolderPanel("Select folder", "Assets", ""));

			var buttonIcon = new Image() { image = EditorGUIUtility.IconContent("d_Folder Icon").image };

			button.style.width = 40f;
			button.style.height = 20f;
			propertyField.style.flexGrow = 1f;
			root.style.flexDirection = FlexDirection.Row;

			button.Add(buttonIcon);
			root.Add(propertyField);
			root.Add(button);

			var textField = new TextField();

			ExecuteLater(propertyField, () => textField = propertyField.Q<TextField>());

			UpdateVisualElement(propertyField, () =>
			{
				if (folderPathAttribute.GetRelativePath && !string.IsNullOrEmpty(folderPath))
				{
					string projectRoot = Application.dataPath[..^"Assets".Length];

					folderPath = Path.GetRelativePath(projectRoot, folderPath);
				}

				if (property.hasMultipleDifferentValues)
					return;

				textField.value = folderPath;
				property.stringValue = folderPath;
				property.serializedObject.ApplyModifiedProperties();
			});

			return root;
		}
	}
}
