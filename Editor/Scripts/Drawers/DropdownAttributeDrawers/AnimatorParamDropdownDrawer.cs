using EditorAttributes.Editor.Utility;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(AnimatorParamDropdownAttribute))]
	public class AnimatorParamDropdownDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var animatorParamAttribute = attribute as AnimatorParamDropdownAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();

			if (property.propertyType == SerializedPropertyType.String)
			{
				var animatorParameters = GetAnimatorParams(animatorParamAttribute, property, errorBox);

				var dropdownField = IsCollectionValid(animatorParameters) ? new DropdownField(property.displayName, animatorParameters, GetDropdownDefaultValue(animatorParameters, property))
					: new DropdownField(property.displayName, new List<string>() { "NULL" }, 0);

				dropdownField.tooltip = property.tooltip;
				dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

				AddPropertyContextMenu(dropdownField, property);

				dropdownField.RegisterValueChangedCallback(callback =>
				{
					if (!property.hasMultipleDifferentValues)
					{
						property.stringValue = callback.newValue;
						property.serializedObject.ApplyModifiedProperties();
					}
				});

				dropdownField.TrackPropertyValue(property, (trackedProperty) =>
				{
					if (dropdownField.choices.Contains(trackedProperty.stringValue))
					{
						dropdownField.SetValueWithoutNotify(trackedProperty.stringValue);
					}
					else
					{
						Debug.LogWarning($"The value <b>{trackedProperty.stringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a valid animator parameter.", trackedProperty.serializedObject.targetObject);
					}
				});

				if (dropdownField.value != "NULL")
				{
					dropdownField.showMixedValue = property.hasMultipleDifferentValues;

					if (!property.hasMultipleDifferentValues)
					{
						property.stringValue = dropdownField.value;
						property.serializedObject.ApplyModifiedProperties();
					}
				}

				root.Add(dropdownField);

				ExecuteLater(dropdownField, () => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

				UpdateVisualElement(dropdownField, () =>
				{
					var animatorParams = GetAnimatorParams(animatorParamAttribute, property, errorBox);

					if (IsCollectionValid(animatorParams))
						dropdownField.choices = animatorParams;
				});
			}
			else
			{
				errorBox.text = "The AnimatorParamDropdown attribute can only be attached to string fields";
			}

			DisplayErrorBox(root, errorBox);

			return root;
		}

		private List<string> GetAnimatorParams(AnimatorParamDropdownAttribute animatorParamAttribute, SerializedProperty property, HelpBox errorBox)
		{
			var paramList = new List<string>();

			var memberInfo = ReflectionUtility.GetValidMemberInfo(animatorParamAttribute.AnimatorFieldName, property);
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == typeof(Animator))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, property) as Animator;

				if (memberInfoValue != null && memberInfoValue.runtimeAnimatorController != null)
				{
					// Hack for having the animator refesh its parameters when editing them in edit mode otherwise the parameters array will be empty
					var editorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(memberInfoValue.runtimeAnimatorController));

					foreach (var parameter in editorController.parameters)
						paramList.Add(parameter.name);
				}
				else
				{
					errorBox.text = $"The Animator or Animator Controller is null, make sure they are assigned";
					return null;
				}
			}
			else
			{
				errorBox.text = $"The provided field \"{animatorParamAttribute.AnimatorFieldName}\" is not of type Animator";
				return null;
			}

			return paramList;
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var dropdown = element as DropdownField;

			if (dropdown.choices.Contains(clipboardValue))
			{
				base.PasteValue(element, property, clipboardValue);
				dropdown.SetValueWithoutNotify(clipboardValue);
			}
			else
			{
				Debug.LogWarning($"Could not paste value \"{clipboardValue}\" since is not availiable as an option in the dropdown");
			}
		}

		private string GetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
		{
			var propertyStringValue = property.stringValue;

			return collectionValues.Contains(propertyStringValue) ? propertyStringValue : collectionValues[0];
		}
	}
}
