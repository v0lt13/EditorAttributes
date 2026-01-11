using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The FolderPath Attribute can only be attached to a string", HelpBoxMessageType.Error);

            var folderPathAttribute = attribute as FolderPathAttribute;
            string folderPath = property.stringValue;

            VisualElement root = new();
            PropertyField propertyField = CreatePropertyField(property);
            Button button = new(() => folderPath = EditorUtility.OpenFolderPanel("Select folder", "Assets", ""));
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
                    if (folderPathAttribute.GetRelativePath && !string.IsNullOrEmpty(folderPath))
                    {
                        string projectRoot = Application.dataPath[..^"Assets".Length];

                        folderPath = Path.GetRelativePath(projectRoot, folderPath);
                    }

                    if (property.hasMultipleDifferentValues)
                        return;

                    propertyField.Q<TextField>().value = folderPath;
                    property.stringValue = folderPath;
                    property.serializedObject.ApplyModifiedProperties();
                });
            });

            return root;
        }
    }
}
