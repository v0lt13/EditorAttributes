using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PrefixAttribute))]
    public class PrefixDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var prefixAttribute = attribute as PrefixAttribute;
            var prefixText = GetDynamicString(prefixAttribute.Prefix, property, prefixAttribute);

			var textSize = EditorStyles.miniLabel.CalcSize(new GUIContent(prefixText));

			EditorGUI.PrefixLabel(new Rect(EditorGUIUtility.labelWidth - textSize.x - prefixAttribute.Offset, position.y, position.width, position.height), new GUIContent(prefixText), EditorStyles.miniLabel);
		    DrawProperty(position, property, label);
    	}
    }
}
