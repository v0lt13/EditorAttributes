using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var conditionalAttribute = attribute as ConditionalFieldAttribute;

			eventDrawer ??= new UnityEventDrawer();

			if (CanDrawProperty(conditionalAttribute, property, out int invalidPropertyIndex))
			{
				try
				{
					eventDrawer.OnGUI(position, property, label);
				}
				catch (NullReferenceException)
				{
					EditorGUI.PropertyField(position, property, label);
				}
			}
			else if (invalidPropertyIndex != -1)
			{
				EditorGUILayout.HelpBox($"The provided condition \"{conditionalAttribute.booleanNames[invalidPropertyIndex]}\" is not a valid boolean", MessageType.Warning);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var conditionalAttribute = attribute as ConditionalFieldAttribute;

			eventDrawer ??= new UnityEventDrawer();

			if (CanDrawProperty(conditionalAttribute, property, out _))
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

		private bool CanDrawProperty(ConditionalFieldAttribute attribute, SerializedProperty property, out int invalidPropertyIndex)
		{
			invalidPropertyIndex = -1;

			for (int i = 0; i < attribute.booleanNames.Length; i++)
			{
				var condition = attribute.booleanNames[i];
				var serializedProperty = property.serializedObject.FindProperty(condition);

				if (serializedProperty != null && serializedProperty.propertyType == SerializedPropertyType.Boolean)
				{
					var propertyValue = serializedProperty.boolValue;

					if (!(attribute.negatedValues == null || attribute.negatedValues.Length == 0))
					{
						if (attribute.negatedValues[i]) propertyValue = !propertyValue; 
					}

					switch (attribute.conditionType)
					{
						case ConditionType.AND:
							if (!propertyValue) return false;
							continue;

						case ConditionType.OR:
							if (propertyValue) return true;
							continue;

						case ConditionType.NAND:
							if (!propertyValue) return true;
							continue;

						case ConditionType.NOR:
							if (propertyValue) return false;
							continue;
					}
				}
				else
				{
					invalidPropertyIndex = i;
					return false;
				}
			}

			return attribute.conditionType switch
			{
				ConditionType.AND => true,
				ConditionType.NOR => true,
				_ => false,
			};
		}
	}
}
