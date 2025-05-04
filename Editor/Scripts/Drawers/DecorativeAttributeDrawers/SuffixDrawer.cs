using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(SuffixAttribute))]
    public class SuffixDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var suffixAttribute = attribute as SuffixAttribute;
			
			var root = new VisualElement();
			var errorBox = new HelpBox();
			var propertyField = CreatePropertyField(property);

			var suffixLabel = new Label() 
			{
				style = {
					fontSize = 12,
					maxWidth = 200f,
					marginLeft = suffixAttribute.Offset,
					unityTextAlign = TextAnchor.MiddleRight,
					alignSelf = Align.Center,
					overflow = Overflow.Hidden
				}
			};

			root.style.flexDirection = FlexDirection.Row;
			propertyField.style.flexGrow = 1f;
			suffixLabel.style.color = suffixLabel.style.color = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR : Color.gray;

			root.Add(propertyField);
			root.Add(suffixLabel);

			UpdateVisualElement(suffixLabel, () =>
			{
				suffixLabel.text = GetDynamicString(suffixAttribute.Suffix, property, suffixAttribute, errorBox);
				DisplayErrorBox(root, errorBox);
			});

			return root;
		}
	}
}
