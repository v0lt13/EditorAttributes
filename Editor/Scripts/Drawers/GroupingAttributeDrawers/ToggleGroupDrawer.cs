using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ToggleGroupAttribute))]
    public class ToggleGroupDrawer : GroupDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var toggleGroup = attribute as ToggleGroupAttribute;
            string foldoutSaveKey = CreatePropertySaveKey(property, "IsToggleGroupFolded");
            string toggleSaveKey = CreatePropertySaveKey(property, "IsToggleGroupToggled");

            VisualElement root = new();

            Foldout foldout = new()
            {
                text = toggleGroup.GroupName,
                tooltip = property.tooltip,
                style = { unityFontStyleAndWeight = FontStyle.Bold },
                value = EditorPrefs.GetBool(foldoutSaveKey)
            };

            Toggle toggleBox = new()
            {
                text = "",
                style = { marginRight = 10f },
                value = property.propertyType == SerializedPropertyType.Boolean ? property.boolValue : EditorPrefs.GetBool(toggleSaveKey)
            };

            foldout.contentContainer.SetEnabled(toggleBox.value);

            if (toggleGroup.DrawInBox)
                ApplyBoxStyle(foldout.contentContainer);

            root.Add(toggleBox);

            foreach (string variableName in toggleGroup.FieldsToGroup)
            {
                VisualElement groupProperty = CreateGroupProperty(variableName, property);
                groupProperty.style.unityFontStyleAndWeight = FontStyle.Normal;

                foldout.Add(groupProperty);
            }

            toggleBox.RegisterValueChangedCallback((callback) =>
            {
                if (property.propertyType == SerializedPropertyType.Boolean)
                {
                    property.boolValue = callback.newValue;
                    property.serializedObject.ApplyModifiedProperties();
                }
                else
                {
                    EditorPrefs.SetBool(toggleSaveKey, callback.newValue); // The value is already serialized via the property, there is no point in saving it.
                }

                foldout.contentContainer.SetEnabled(callback.newValue);
            });

            root.Add(foldout);

            foldout.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var toggle = foldout.Q<Toggle>();

                toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

                var parentElement = foldout.Q<Label>().parent;

                parentElement.Insert(1, toggleBox);

                // Register this callback later since value changed callbacks are called on inspector initalization and we don't want to save values on initalization
                foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));
            });

            if (property.propertyType == SerializedPropertyType.Boolean)
                UpdateVisualElement(toggleBox, () => toggleBox.value = property.boolValue);

            return root;
        }
    }
}
