using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(GUIColorAttribute))]
    public class GUIColorDrawer : DecoratorDrawer
    {
    	public override void OnGUI(Rect position)
    	{
			var guiColorAttribute = attribute as GUIColorAttribute;

			if (ColorUtility.TryParseHtmlString(guiColorAttribute.HexColor, out Color color))
			{
				GUI.color = color;
			}
			else if (!string.IsNullOrEmpty(guiColorAttribute.HexColor))
			{
				EditorGUILayout.HelpBox($"The provided value {guiColorAttribute.HexColor} is not a valid Hex color", MessageType.Error);
			}
			else
			{
				GUI.color = PropertyDrawerBase.GUIColorToColor(guiColorAttribute);
			}
    	}

		public override float GetHeight() => 0f;
	}
}
