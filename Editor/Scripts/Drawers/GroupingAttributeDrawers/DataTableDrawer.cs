using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EditorAttributes.Editor.Utility;
using System.Threading.Tasks;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DataTableAttribute))]
    public class DataTableDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!IsSupportedPropertyType(property))
                return new HelpBox("The DataTable Attribute can only be attached to serialized structs or classes and collections containing them", HelpBoxMessageType.Error);

            var dataTableAttribute = attribute as DataTableAttribute;
            VisualElement root = new();

            property.isExpanded = true;

            if (dataTableAttribute.DrawInBox)
                ApplyBoxStyle(root);

            root.style.flexDirection = FlexDirection.Row;

            Label label = new(property.displayName)
            {
                tooltip = property.tooltip,
                style = {
                    overflow = Overflow.Hidden,
                    unityFontStyleAndWeight = FontStyle.Bold,
                    marginRight = 50f,
                    maxWidth = 100f,
                    width = 100f,
                    alignSelf = Align.Center,
                    color = EditorExtension.GLOBAL_COLOR
                }
            };

            root.Add(label);

            SerializedProperty serializedProperty = property.Copy();

            int initialDepth = serializedProperty.depth;

            while (serializedProperty.NextVisible(true) && serializedProperty.depth > initialDepth)
            {
                if (serializedProperty.depth >= initialDepth + 2) // Skip the X Y Z W properties that are inside Vectors since we draw the vector field ourself
                    continue;

                VisualElement tableColumn = new();
                tableColumn.style.flexGrow = 1f;
                tableColumn.style.flexBasis = 0.1f;

                bool isFoldoutProperty = serializedProperty.propertyType is SerializedPropertyType.Generic or SerializedPropertyType.Vector4;

                // Draw column labels
                if (dataTableAttribute.ShowLabels && IsFirstCollectionElement(property) && !isFoldoutProperty)
                {
                    Label propertyLabel = new(serializedProperty.displayName);

                    propertyLabel.style.color = EditorExtension.GLOBAL_COLOR;
                    propertyLabel.style.overflow = Overflow.Hidden;
                    propertyLabel.tooltip = serializedProperty.tooltip;

                    tableColumn.Add(propertyLabel);
                }

                PropertyField propertyField = new(serializedProperty);

                if (isFoldoutProperty)
                    propertyField.style.marginLeft = 5f;

                propertyField.style.flexGrow = 1f;
                propertyField.style.marginRight = 10f;
                propertyField.RemoveFromClassList(BaseField<Void>.alignedFieldUssClassName);

                if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
                    ColorUtils.ApplyColor(propertyField, EditorExtension.GLOBAL_COLOR, 100);

                // Hide all property labels, except for the ones inside types with foldout displays (collections, vector 4, serialized objects)
                propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
                {
                    if (isFoldoutProperty)
                    {
                        UpdateFoldoutPropertyLabels(propertyField);

                        // If the foldout is folded when the editor is drawn, the labels will not be found by the query so we update them when the foldout is unfolded
                        propertyField.Q<Foldout>().RegisterValueChangedCallback((callback) => UpdateFoldoutPropertyLabels(propertyField));
                    }
                    else
                    {
                        RemoveGeneratedPropertyLabels(propertyField);
                    }
                });

                // If new collection entries are added they will not have the flexGrow value, so we listen for changes to update the labels on the new entries
                if (serializedProperty.isArray && serializedProperty.propertyType != SerializedPropertyType.String) // Strings are considered arrays but we don't want to include them
                    propertyField.RegisterValueChangeCallback((callback) => UpdateFoldoutPropertyLabels(propertyField));

                tableColumn.Add(propertyField);
                root.Add(tableColumn);
            }

            return root;
        }

        protected override bool IsSupportedPropertyType(SerializedProperty property) => property.propertyType == SerializedPropertyType.Generic;

        private bool IsFirstCollectionElement(SerializedProperty property)
        {
            if (property.propertyPath.Contains("Array"))
                return property.propertyPath.Contains("data[0]");

            return true;
        }

        private async void UpdateFoldoutPropertyLabels(PropertyField propertyField)
        {
            // Wait 1 milisecond to give time for the labels to draw in the inspector before we query them, otherwise they will not be found
            await Task.Delay(1);

            var labels = propertyField.Query<Label>(className: PropertyField.labelUssClassName).ToList();

            foreach (var label in labels)
            {
                label.style.flexGrow = 1f;
                label.style.maxWidth = 100f;
            }
        }

        private async void RemoveGeneratedPropertyLabels(PropertyField propertyField)
        {
            await Task.Delay(1);

            var labels = propertyField.Query<Label>(className: PropertyField.labelUssClassName).ToList();
            labels.AddRange(propertyField.Query<Label>(className: PopupField<Void>.labelUssClassName).ToList());

            foreach (var label in labels)
                label.style.display = DisplayStyle.None;
        }
    }
}
