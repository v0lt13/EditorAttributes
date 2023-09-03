using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowFieldAttribute))]
    public class ShowFieldDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var showAttribute = attribute as ShowFieldAttribute;

			var conditionalProperty = property.serializedObject.FindProperty(showAttribute.conditionName);

			eventDrawer ??= new UnityEventDrawer();

			if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
			{
				bool conditionalValue = conditionalProperty.boolValue;

				DrawProperty(conditionalValue, position, property, label);
			}
			else if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Enum)
			{
				bool conditionalValue = conditionalProperty.intValue == showAttribute.enumValue;

				DrawProperty(conditionalValue, position, property, label);
			}
			else
			{
				EditorGUILayout.HelpBox($"The provided condition \"{showAttribute.conditionName}\" is not a valid boolean or an enum", MessageType.Warning);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var showAttribute = attribute as ShowFieldAttribute;

			var conditionalProperty = property.serializedObject.FindProperty(showAttribute.conditionName);

			eventDrawer ??= new UnityEventDrawer();

			if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
			{
				bool conditionalValue = conditionalProperty.boolValue;

				return GetPropertyHeight(conditionalValue, property, label);
			}
			else if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Enum)
			{
				bool conditionalValue = conditionalProperty.intValue == showAttribute.enumValue;

				return GetPropertyHeight(conditionalValue, property, label);
			}

			return GetCorrectPropertyHeight(property, label);
		}

		private float GetPropertyHeight(bool conditionalValue, SerializedProperty property, GUIContent label)
		{
			if (conditionalValue)
			{
				return GetCorrectPropertyHeight(property, label);
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
			}
		}

		private float GetCorrectPropertyHeight(SerializedProperty property, GUIContent label)
		{
			try
			{
				return eventDrawer.GetPropertyHeight(property, label);
			}
			catch (NullReferenceException)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
		}

		private void DrawProperty(bool conditionalValue, Rect position, SerializedProperty property, GUIContent label)
		{
			if (conditionalValue)
			{
				try
				{
					eventDrawer.OnGUI(position, property, label);
				}
				catch (NullReferenceException)
				{
					EditorGUI.PropertyField(position, property, label, true);
				}
			}
		}
	}
}
