using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(LineAttribute))]
    public class LineDrawer : DecoratorDrawer
    {
    	public override void OnGUI(Rect position)
    	{
            var lineAttribute = attribute as LineAttribute;

			var indentedRect = EditorGUI.IndentedRect(position);
			indentedRect.y += EditorGUIUtility.singleLineHeight / 3f;

			if (ColorUtility.TryParseHtmlString(lineAttribute.HexColor, out Color color))
			{
				EditorGUI.DrawRect(new Rect(position.x, indentedRect.y, position.width, 3f), color);
				return;
			}
			else if (!string.IsNullOrEmpty(lineAttribute.HexColor))
			{
				EditorGUILayout.HelpBox($"The provided value {lineAttribute.HexColor} is not a valid Hex color", MessageType.Error);
			}

			EditorGUI.DrawRect(new Rect(position.x, indentedRect.y, position.width, 3f), GetLineColor(lineAttribute));
    	}

        private Color GetLineColor(LineAttribute attribute)
        {
			return attribute.LineColor switch
			{
				LineColor.White => new Color(Color.white.r, Color.white.g, Color.white.b, attribute.A),
				LineColor.Black => new Color(Color.black.r, Color.black.g, Color.black.b, attribute.A),
				LineColor.Gray => new Color(Color.gray.r, Color.gray.g, Color.gray.b, attribute.A),
				LineColor.Red => new Color(Color.red.r, Color.red.g, Color.red.b, attribute.A),
				LineColor.Green => new Color(Color.green.r, Color.green.g, Color.green.b, attribute.A),
				LineColor.Blue => new Color(Color.blue.r, Color.blue.g, Color.blue.b, attribute.A),
				LineColor.Cyan => new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, attribute.A),
				LineColor.Magenta => new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, attribute.A),
				LineColor.Yellow => new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, attribute.A),
				LineColor.Orange => new Color(1f, 149f / 255f, 0f, attribute.A),
				LineColor.Brown => new Color(161f / 255f, 62f / 255f, 0f, attribute.A),
				LineColor.Purple => new Color(158f / 255f, 5f / 255f, 247f / 255f, attribute.A),
				LineColor.Pink => new Color(247f / 255f, 5f / 255f, 171f / 255f, attribute.A),
				LineColor.Lime => new Color(145f / 255f, 1f, 0f, attribute.A),
				_ => new Color(attribute.R / 255f, attribute.G / 255f, attribute.B / 255f, attribute.A)
			};
		}
    }
}
