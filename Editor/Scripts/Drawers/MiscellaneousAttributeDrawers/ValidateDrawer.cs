using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ValidateAttribute))]
    public class ValidateDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var validateAttribute = attribute as ValidateAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(validateAttribute.ConditionName, property);

			var propertyField = CreatePropertyField(property);

			root.Add(propertyField);

			var errorBox = new HelpBox();
			var helpBox = new HelpBox(validateAttribute.ValidationMessage, (HelpBoxMessageType)validateAttribute.Severety);

			if (CanApplyGlobalColor)
			{
				helpBox.style.color = EditorExtension.GLOBAL_COLOR;
				helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
			}

			UpdateVisualElement(root, () =>
			{
				DisplayErrorBox(root, errorBox);

				if (GetConditionValue(conditionalProperty, validateAttribute, property, helpBox, errorBox))
				{
					AddElement(root, helpBox);
				}
				else
				{
					RemoveElement(root, helpBox);
				}
			});

			return root;
		}

		private bool GetConditionValue(MemberInfo memberInfo, ValidateAttribute validateAttribute, SerializedProperty serializedProperty, HelpBox helpBox, HelpBox errorBox)
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				errorBox.text = $"The provided condition \"{validateAttribute.ConditionName}\" could not be found";
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue == null)
					return false;

				return (bool)memberInfoValue;
			}
			else if (memberInfoType == typeof(ValidationCheck))
			{
				if (ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty) is not ValidationCheck memberInfoValue)
					return false;

				if (validateAttribute.ValidationMessage != null)
				{
					errorBox.text = "The condition uses <b>ValidationCheck</b> but the attribute still uses the constructor with the <b>validationMessage</b> parameter which will be overriden";
					errorBox.messageType = HelpBoxMessageType.Info;
				}

				helpBox.text = memberInfoValue.ValidationMessage;
				helpBox.messageType = (HelpBoxMessageType)memberInfoValue.Severety;

				return !memberInfoValue.PassedCheck;
			}

			errorBox.text = $"The provided condition \"{validateAttribute.ConditionName}\" is not a valid boolean or ValidationCheck type";

			return false;
		}
	}
}
