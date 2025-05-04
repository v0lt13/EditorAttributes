using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var filePathAttribute = attribute as FilePathAttribute;
			var root = new VisualElement();

			if (property.propertyType != SerializedPropertyType.String)
			{
				root.Add(new HelpBox("The FilePath Attribute can only be attached to a string", HelpBoxMessageType.Error));
				return root;
			}

			var filePath = property.stringValue;

			var propertyField = CreatePropertyField(property);
			var button = new Button(() => filePath = EditorUtility.OpenFilePanel("Select file", "Assets", filePathAttribute.Filters));

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
				if (filePathAttribute.GetRelativePath && !string.IsNullOrEmpty(filePath) && Path.IsPathFullyQualified(filePath))
				{
					string projectRoot = Application.dataPath[..^"Assets".Length];

					filePath = Path.GetRelativePath(projectRoot, filePath);
				}

				if (property.hasMultipleDifferentValues)
					return;

				textField.value = filePath;
				property.stringValue = filePath;
				property.serializedObject.ApplyModifiedProperties();
			});

			return root;
		}
	}
}
