using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(VerticalGroupAttribute))]
    public class VerticalGroupDrawer : GroupDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var verticalGroup = attribute as VerticalGroupAttribute;
            GroupBox groupBox = new(verticalGroup.GroupName);

            if (verticalGroup.DrawInBox)
                ApplyBoxStyle(groupBox);

            foreach (string variableName in verticalGroup.FieldsToGroup)
            {
                VisualElement groupProperty = CreateGroupProperty(variableName, property);
                groupBox.Add(groupProperty);
            }

            groupBox.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var groupLabel = groupBox.Q<Label>(className: GroupBox.labelUssClassName);

                if (groupLabel == null)
                    return;

                groupLabel.style.marginTop = 0f;
                groupLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            });

            return groupBox;
        }
    }
}
