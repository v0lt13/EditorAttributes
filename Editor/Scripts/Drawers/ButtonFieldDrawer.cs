using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ButtonFieldAttribute))]
    public class ButtonFieldDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var buttonFieldAttribute = attribute as ButtonFieldAttribute;
            var target = property.serializedObject.targetObject;

            var function = ReflectionUtility.FindFunction(buttonFieldAttribute.FunctionName, target);
			var functionParameters = function.GetParameters();

            if (functionParameters.Length == 0)
            {
                if (GUILayout.Button(string.IsNullOrWhiteSpace(buttonFieldAttribute.ButtonLabel) ? function.Name : buttonFieldAttribute.ButtonLabel, GUILayout.Height(buttonFieldAttribute.ButtonHeight)))
                    function.Invoke(target, null);
            }
            else
            {
                EditorGUILayout.HelpBox("Function cannot have parameters", MessageType.Error);
            }
		}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;
	}
}
