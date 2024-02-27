using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    public class TitleDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var titleAttribute = attribute as TitleAttribute;
			var titleText = GetDynamicString(titleAttribute.Title, property, titleAttribute);
			var titleStyle = new GUIStyle
			{
				fontSize = titleAttribute.TitleSize,
				alignment = titleAttribute.Alignment,
				richText = true
			};

			titleStyle.normal.textColor = GUI.contentColor;

			EditorGUI.LabelField(new Rect(position.x, position.y, position.width, titleAttribute.TitleSize + 2f), titleText, titleStyle);

            if (titleAttribute.DrawLine) EditorGUI.DrawRect(new Rect(position.x, position.yMax - 2f, position.width, 1f), GUI.color);

			EditorGUILayout.PropertyField(property);
    	}

		protected override float GetCorrectPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var titleAttribute = attribute as TitleAttribute;

            return titleAttribute.DrawLine ? titleAttribute.TitleSize + 2f : titleAttribute.TitleSize;
		}
	}
}
