using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;
using System.Reflection;

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

			var propertyField = DrawProperty(property);

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
				if (GetConditionValue(conditionalProperty, validateAttribute, property, errorBox))
				{
					root.Add(helpBox);
				}
				else
				{
					RemoveElement(root, helpBox);
				}

				DisplayErrorBox(root, errorBox);
			});

			return root;
		}

		private bool GetConditionValue(MemberInfo memberInfo, ValidateAttribute validateAttribute, SerializedProperty serializedProperty, HelpBox errorBox)
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

			errorBox.text = $"The provided condition \"{validateAttribute.ConditionName}\" is not a valid boolean";

			return false;
		}
	}
}
