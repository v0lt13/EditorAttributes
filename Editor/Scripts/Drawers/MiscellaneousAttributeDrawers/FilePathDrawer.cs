using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The FilePath Attribute can only be attached to a string", HelpBoxMessageType.Error);

            var filePathAttribute = attribute as FilePathAttribute;
            string filePath = property.stringValue;

            VisualElement root = new();
            PropertyField propertyField = CreatePropertyField(property);
            Button button = new(() => filePath = EditorUtility.OpenFilePanel("Select file", "Assets", filePathAttribute.Filters));
            Image buttonIcon = new() { image = EditorGUIUtility.IconContent("d_Folder Icon").image };

            button.style.width = 40f;
            button.style.height = 20f;
            propertyField.style.flexGrow = 1f;
            root.style.flexDirection = FlexDirection.Row;

            button.Add(buttonIcon);
            root.Add(propertyField);
            root.Add(button);

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                UpdateVisualElement(propertyField, () =>
                {
                    if (filePathAttribute.GetRelativePath && !string.IsNullOrEmpty(filePath) && Path.IsPathFullyQualified(filePath))
                    {
                        string projectRoot = Application.dataPath[..^"Assets".Length];

                        filePath = Path.GetRelativePath(projectRoot, filePath);
                    }

                    if (property.hasMultipleDifferentValues)
                        return;

                    propertyField.Q<TextField>().value = filePath;
                    property.stringValue = filePath;
                    property.serializedObject.ApplyModifiedProperties();
                });
            });

            return root;
        }
    }
}
