using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(LineAttribute))]
    public class LineDrawer : DecoratorDrawer
    {
		public override VisualElement CreatePropertyGUI()
		{
			var lineAttribute = attribute as LineAttribute;

			var root = new VisualElement();
			var line = new VisualElement();
			var errorBox = new HelpBox();

			line.style.marginBottom = 5f;
			line.style.marginTop = 5f;
			line.style.height = lineAttribute.LineThickness;			
			line.style.backgroundColor = ColorUtils.GetColorFromAttribute(lineAttribute, lineAttribute.A, errorBox);

			root.Add(line);

			return root;
		}
	}
}
