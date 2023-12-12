using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SuffixAttribute))]
    public class SuffixDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var suffixAttribute = attribute as SuffixAttribute;

			var textSize = EditorStyles.miniLabel.CalcSize(new GUIContent(suffixAttribute.Suffix));

			EditorGUIUtility.fieldWidth -= position.xMax - textSize.x;

            DrawProperty(new Rect(position.x, position.y, position.width - textSize.x - suffixAttribute.Offset, position.height), property, label);

			GUI.Label(new Rect(position.xMax - textSize.x, position.y, position.width, position.height), suffixAttribute.Suffix, EditorStyles.miniLabel);
		}
	}
}
