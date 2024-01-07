using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(InlineButtonAttribute))]
    public class InlineButtonDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var inlineButtonAttribute = attribute as InlineButtonAttribute;
            var buttonLabel = inlineButtonAttribute.ButtonLabel == string.Empty ? inlineButtonAttribute.FunctionName : inlineButtonAttribute.ButtonLabel;

            var methodInfo = ReflectionUtility.FindFunction(inlineButtonAttribute.FunctionName, property.serializedObject.targetObject);

            if (methodInfo.GetParameters().Length > 0)
            {
                EditorGUILayout.HelpBox("The function cannot have parameters", MessageType.Error);
                return;
            }

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - inlineButtonAttribute.ButtonWidth, position.height), property);

            if (GUI.Button(new Rect(position.xMax - inlineButtonAttribute.ButtonWidth + 2f, position.y, inlineButtonAttribute.ButtonWidth, position.height), buttonLabel))
                methodInfo.Invoke(property.serializedObject.targetObject, null);
    	}
	}
}
