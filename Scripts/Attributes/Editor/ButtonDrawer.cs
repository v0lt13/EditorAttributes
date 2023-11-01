using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawerBase
    {
		object[] parameterValues;
		bool foldout;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var buttonAttribute = attribute as ButtonAttribute;

			var functionName = buttonAttribute.FunctionName;
			var target = property.serializedObject.targetObject;
			var function = FindFunction(functionName, property);

			if (function == null)
			{
				EditorGUILayout.HelpBox($"The provided function \"{functionName}\" could not be found. Make sure is public", MessageType.Error);
				return;
			}

			var functionParameters = function.GetParameters();

			if (parameterValues == null || parameterValues.Length != functionParameters.Length)
			{
				parameterValues = new object[functionParameters.Length];

				for (int i = 0; i < functionParameters.Length; i++) parameterValues[i] = functionParameters[i].DefaultValue;
			}

			if (functionParameters.Length > 0f)
			{
				foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Parameters");

				if (foldout)
				{
					for (int i = 0; i < functionParameters.Length; i++) parameterValues[i] = DrawField(functionParameters[i].ParameterType, functionParameters[i].Name, parameterValues[i]);
				}

				EditorGUILayout.EndFoldoutHeaderGroup();

				EditorGUILayout.Space(10f);
			}
			
			if (GUI.Button(position, string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel)) function.Invoke(target, parameterValues);
		}
	}
}
