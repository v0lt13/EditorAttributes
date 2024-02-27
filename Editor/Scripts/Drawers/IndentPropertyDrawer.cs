using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(IndentPropertyAttribute))]
    public class IndentPropertyDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var indentPropertyAttribute = attribute as IndentPropertyAttribute;

            using (new EditorGUI.IndentLevelScope(indentPropertyAttribute.IndentLevel))
                DrawProperty(position, property, label);
		}
    }
}
