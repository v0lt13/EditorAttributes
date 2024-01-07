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

			EditorGUI.DrawRect(new Rect(position.x, indentedRect.y, position.width, 3f), PropertyDrawerBase.GUIColorToColor(lineAttribute, lineAttribute.A));
    	}
    }
}
