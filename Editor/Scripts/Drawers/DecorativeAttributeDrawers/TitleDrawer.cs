using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using ColorUtility = EditorAttributes.Editor.Utility.ColorUtility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var titleAttribute = attribute as TitleAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();
			var title = new Label();

			UpdateVisualElement(() =>
			{
				title.text = GetDynamicString(titleAttribute.Title, property, titleAttribute, errorBox);
				DisplayErrorBox(root, errorBox);
			});

			title.style.fontSize = titleAttribute.TitleSize;
			title.style.unityTextAlign = titleAttribute.Alignment;
			title.style.marginBottom = titleAttribute.TitleSpace;

			if (titleAttribute.DrawLine)
			{
				var colorWithAlpha = ColorUtility.GetPropertyColor(property, 0.5f);
				var color = ColorUtility.GetPropertyColor(property);

				if (color.HasValue)
					title.style.color = color.Value;

				title.style.borderBottomColor = colorWithAlpha ?? Color.gray;
				title.style.borderBottomWidth = 2f;
			}

			root.Add(title);
			root.Add(DrawProperty(property));

			return root;
		}
	}
}
