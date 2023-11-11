using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SufixAttribute))]
    public class SufixDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var sufixAttribute = attribute as SufixAttribute;

			var textSize = GUI.skin.GetStyle("miniLabel").CalcSize(new GUIContent(sufixAttribute.Sufix));

			EditorGUIUtility.fieldWidth -= position.xMax - textSize.x;

            DrawProperty(new Rect(position.x, position.y, position.width - textSize.x - sufixAttribute.Offset, position.height), property, label);

			GUI.Label(new Rect(position.xMax - textSize.x, position.y, position.width, position.height), sufixAttribute.Sufix, new GUIStyle(EditorStyles.miniLabel));
		}
	}
}
