using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var buttonAttribute = attribute as ButtonAttribute;

			var functionName = buttonAttribute.functionName;
			var target = property.serializedObject.targetObject;
			var function = target.GetType().GetMethod(functionName);

			if (function == null)
			{
				EditorGUILayout.HelpBox($"The provided function \"{functionName}\" could not be found. Make sure is public", MessageType.Error);
				return;
			}

			if (function.GetParameters().Length > 0f)
			{
				EditorGUILayout.HelpBox("Function cannot have parameters", MessageType.Error);
				return;
			}

			if (GUI.Button(position, buttonAttribute.buttonLabel == string.Empty ? function.Name : buttonAttribute.buttonLabel)) function.Invoke(target, null);
		}
	}
}
