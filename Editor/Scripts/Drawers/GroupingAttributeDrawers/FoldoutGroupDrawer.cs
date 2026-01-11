using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FoldoutGroupAttribute))]
    public class FoldoutGroupDrawer : GroupDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var foldoutGroup = attribute as FoldoutGroupAttribute;
            string foldoutSaveKey = CreatePropertySaveKey(property, "IsFoldoutGroupFolded");

            Foldout foldout = new()
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold },
                text = foldoutGroup.GroupName,
                tooltip = property.tooltip,
                value = EditorPrefs.GetBool(foldoutSaveKey)
            };

            if (foldoutGroup.DrawInBox)
                ApplyBoxStyle(foldout.contentContainer);

            foreach (string variableName in foldoutGroup.FieldsToGroup)
            {
                VisualElement groupProperty = CreateGroupProperty(variableName, property);
                groupProperty.style.unityFontStyleAndWeight = FontStyle.Normal;

                foldout.Add(groupProperty);
            }

            foldout.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var toggle = foldout.Q<Toggle>();
                toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

                // Register this callback later since value changed callbacks are called on inspector initalization and we don't want to save values on initalization
                foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));
            });

            return foldout;
        }
    }
}
