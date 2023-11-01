using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var conditionalAttribute = attribute as ConditionalFieldAttribute;

			switch (conditionalAttribute.ConditionResult)
			{
				case ConditionResult.ShowHide:
					if (CanDrawProperty(conditionalAttribute, conditionalAttribute.BooleanNames, property, true)) DrawProperty(position, property, label);
					break;

				case ConditionResult.EnableDisable:
					GUI.enabled = CanDrawProperty(conditionalAttribute, conditionalAttribute.BooleanNames, property, true);

					DrawProperty(position, property, label);

					GUI.enabled = true;
					break;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var conditionalAttribute = attribute as ConditionalFieldAttribute;

			switch (conditionalAttribute.ConditionResult)
			{
				default:
				case ConditionResult.ShowHide:
					if (CanDrawProperty(conditionalAttribute, conditionalAttribute.BooleanNames, property))
					{
						return GetCorrectPropertyHeight(property, label);
					}
					else
					{
						return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
					}

				case ConditionResult.EnableDisable:
					return GetCorrectPropertyHeight(property, label);
			}
		}

		private bool CanDrawProperty(ConditionalFieldAttribute attribute, string[] conditionNames, SerializedProperty property, bool drawErrorBox = false)
		{
			var booleanList = new List<bool>();

			foreach (var conditionName in conditionNames)
			{
				var memberInfo = GetValidMemberInfo(conditionName, property);
				var serializedProperty = property.serializedObject.FindProperty(conditionName);

				if (memberInfo != null && GetMemberInfoType(memberInfo) == typeof(bool))
				{
					var propertyValue = (bool)GetMemberInfoValue(memberInfo, property.serializedObject.targetObject);

					booleanList.Add(propertyValue);
				}
				else if (serializedProperty != null && serializedProperty.propertyType == SerializedPropertyType.Boolean)
				{
					var propertyValue = serializedProperty.boolValue;

					booleanList.Add(propertyValue);
				}
				else
				{
					if (drawErrorBox) EditorGUILayout.HelpBox($"The provided condition \"{conditionName}\" is not a valid boolean", MessageType.Error);
				}
			}

			for (int i = 0; i < booleanList.Count; i++)
			{
				if (!(attribute.NegatedValues == null || attribute.NegatedValues.Length == 0))
				{
					if (attribute.NegatedValues[i]) booleanList[i] = !booleanList[i];
				}

				switch (attribute.ConditionType)
				{
					case ConditionType.AND:
						if (!booleanList[i]) return false;
						continue;

					case ConditionType.OR:
						if (booleanList[i]) return true;
						continue;

					case ConditionType.NAND:
						if (!booleanList[i]) return true;
						continue;

					case ConditionType.NOR:
						if (booleanList[i]) return false;
						continue;
				}
			}

			return attribute.ConditionType switch
			{
				ConditionType.AND => true,
				ConditionType.NOR => true,
				_ => false,
			};
		}
	}
}
