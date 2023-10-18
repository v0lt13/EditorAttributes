using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var disableAttribute = attribute as DisableFieldAttribute;

			eventDrawer ??= new UnityEventDrawer();

			var conditionalProperty = property.serializedObject.FindProperty(disableAttribute.conditionName);

			if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
			{
				bool conditionalValue = conditionalProperty.boolValue;

				DrawProperty(conditionalValue, position, property, label);
			}
			else if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Enum)
			{
				bool conditionalValue = conditionalProperty.intValue == disableAttribute.enumValue;

				DrawProperty(conditionalValue, position, property, label);
			}
			else
			{
				EditorGUILayout.HelpBox($"The provided condition \"{disableAttribute.conditionName}\" is not a valid boolean or enum", MessageType.Warning);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			try
			{
				return eventDrawer.GetPropertyHeight(property, label);
			}
			catch (NullReferenceException)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
		}

		private void DrawProperty(bool condition, Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = !condition;

			try
			{
				eventDrawer.OnGUI(position, property, label);
			}
			catch (NullReferenceException)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			GUI.enabled = true;
		}
	}
}
