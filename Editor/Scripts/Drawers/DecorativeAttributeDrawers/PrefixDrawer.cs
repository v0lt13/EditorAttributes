using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(PrefixAttribute))]
    public class PrefixDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var prefixAttribute = attribute as PrefixAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();
			var propertyField = CreatePropertyField(property);

			var prefixLabel = new Label()
			{
				style = {
					fontSize = 12,
					maxWidth = 200f,
					marginRight = prefixAttribute.Offset,
					unityTextAlign = TextAnchor.MiddleRight,
					alignSelf = Align.FlexEnd,
					overflow = Overflow.Hidden
				}
			};

			prefixLabel.style.color = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR : Color.gray;
			root.Add(propertyField);

			ExecuteLater(root, () =>
			{
				var field = propertyField.Q<Label>();							
				field.Add(prefixLabel);
			});

			UpdateVisualElement(prefixLabel, () =>
			{
				prefixLabel.text = GetDynamicString(prefixAttribute.Prefix, property, prefixAttribute, errorBox);		
				DisplayErrorBox(root, errorBox);
			});

			return root;
		}
    }
}
