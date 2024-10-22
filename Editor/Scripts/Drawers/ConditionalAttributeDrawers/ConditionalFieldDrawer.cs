using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
    public class ConditionalFieldDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var conditionalAttribute = attribute as ConditionalFieldAttribute;

			var root = new VisualElement();
			var propertyField = DrawProperty(property);

			var errorBox = new HelpBox();

			UpdateVisualElement(root, () =>
			{
				var canDrawProperty = CanDrawProperty(conditionalAttribute, conditionalAttribute.BooleanNames, property, errorBox);

				switch (conditionalAttribute.ConditionResult)
				{
					case ConditionResult.ShowHide:
						if (canDrawProperty)
						{
							root.Add(propertyField);
						}
						else
						{
							RemoveElement(root, propertyField);
						}
						break;

					case ConditionResult.EnableDisable:
						propertyField.SetEnabled(canDrawProperty);
						break;
				}

				DisplayErrorBox(root, errorBox);
			});

			root.Add(propertyField);

			return root;
		}

		private bool CanDrawProperty(ConditionalFieldAttribute attribute, string[] conditionNames, SerializedProperty property, HelpBox errorBox)
		{
			var booleanList = new List<bool>();

			foreach (var conditionName in conditionNames)
			{
				var memberInfo = ReflectionUtility.GetValidMemberInfo(conditionName, property);
				var serializedProperty = property.serializedObject.FindProperty(conditionName);

				if (memberInfo == null)
				{
					errorBox.text = $"The provided condition \"{conditionName}\" could not be found";
					continue;
				}

				if (ReflectionUtility.GetMemberInfoType(memberInfo) == typeof(bool))
				{
					var propertyValue = (bool)ReflectionUtility.GetMemberInfoValue(memberInfo, property);

					booleanList.Add(propertyValue);
				}
				else if (serializedProperty != null && serializedProperty.propertyType == SerializedPropertyType.Boolean)
				{
					var propertyValue = serializedProperty.boolValue;

					booleanList.Add(propertyValue);
				}
				else
				{
					errorBox.text = $"The provided condition \"{conditionName}\" is not a valid boolean";
				}
			}

			for (int i = 0; i < booleanList.Count; i++)
			{
				if (!(attribute.NegatedValues == null || attribute.NegatedValues.Length == 0))
				{
					if (attribute.NegatedValues[i]) 
						booleanList[i] = !booleanList[i];
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
