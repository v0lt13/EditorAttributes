using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using ColorUtility = EditorAttributes.Editor.Utility.ColorUtility;

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

			line.style.height = 3f;			
			line.style.marginBottom = 5f;
			line.style.marginTop = 5f;

			if (UnityEngine.ColorUtility.TryParseHtmlString(lineAttribute.HexColor, out Color color))
			{
				line.style.backgroundColor = color;
				root.Add(line);

				return root;
			}
			else if (!string.IsNullOrEmpty(lineAttribute.HexColor))
			{
				root.Add(new HelpBox($"The provided value {lineAttribute.HexColor} is not a valid Hex color", HelpBoxMessageType.Error));
			}

			line.style.backgroundColor = ColorUtility.GUIColorToColor(lineAttribute, lineAttribute.A);
			root.Add(line);

			return root;
		}
	}
}
