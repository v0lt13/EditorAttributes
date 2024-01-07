using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : DecoratorDrawer
    {
    	public override void OnGUI(Rect position)
    	{
            var titleAttribute = attribute as TitleAttribute;
			var titleStyle = new GUIStyle
			{
				fontSize = titleAttribute.TitleSize,
				alignment = titleAttribute.Alignment,
				richText = true
			};

			titleStyle.normal.textColor = GUI.contentColor;

			EditorGUI.LabelField(new Rect(position.x, position.y, position.width, titleAttribute.TitleSize + 2f), titleAttribute.Title, titleStyle);

            if (titleAttribute.DrawLine) EditorGUI.DrawRect(new Rect(position.x, position.yMax - 2f, position.width, 1f), GUI.color);
    	}

		public override float GetHeight()
		{
			var titleAttribute = attribute as TitleAttribute;

            return titleAttribute.DrawLine ? titleAttribute.TitleSize + 2f : titleAttribute.TitleSize;
		}
	}
}
