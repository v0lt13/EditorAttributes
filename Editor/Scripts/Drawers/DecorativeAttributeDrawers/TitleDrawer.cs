using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var titleAttribute = attribute as TitleAttribute;

            VisualElement root = new();
            HelpBox errorBox = new();
            Label title = new();

            title.style.fontSize = titleAttribute.TitleSize;
            title.style.unityTextAlign = titleAttribute.Alignment;
            title.style.marginBottom = titleAttribute.TitleSpace;

            if (titleAttribute.DrawLine)
            {
                Color? colorWithAlpha = ColorUtils.GetPropertyColor(property, 0.5f);
                Color? color = ColorUtils.GetPropertyColor(property);

                if (color.HasValue)
                    title.style.color = color.Value;

                title.style.borderBottomColor = colorWithAlpha ?? Color.gray;
                title.style.borderBottomWidth = titleAttribute.LineThickness;
            }

            root.Add(title);
            root.Add(CreatePropertyField(property));

            UpdateVisualElement(title, () =>
            {
                title.text = GetDynamicString(titleAttribute.Title, property, titleAttribute, errorBox);
                DisplayErrorBox(root, errorBox);
            });

            return root;
        }
    }
}
