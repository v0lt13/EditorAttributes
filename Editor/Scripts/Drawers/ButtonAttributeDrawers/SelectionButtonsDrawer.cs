using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
#if UNITY_6000_0_OR_NEWER
	[Obsolete]
#endif
	[CustomPropertyDrawer(typeof(SelectionButtonsAttribute))]
	public class SelectionButtonsDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			var root = new VisualElement()
			{
				style = {
					paddingTop = 3f,
					paddingBottom = 3f
				}
			};
			var errorBox = new HelpBox();
			
			if (property.propertyType == SerializedPropertyType.Enum)
			{
				var enumType = fieldInfo.FieldType;
				bool isFlagsEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

				if (isFlagsEnum)
				{
					var flagValue = property.enumValueFlag;

					root.Add(DrawEnumFlagButtons(flagValue, property.enumDisplayNames, selectionButtonsAttribute, (value) =>
					{
						property.enumValueFlag = value;
						property.serializedObject.ApplyModifiedProperties();
					}));
				}
				else
				{
					var buttonsValue = property.enumValueIndex;

					root.Add(DrawButtons(buttonsValue, property.enumDisplayNames, selectionButtonsAttribute, (value) =>
					{
						property.enumValueIndex = value;
						property.serializedObject.ApplyModifiedProperties();
					}));
				}
			}
			else if (property.propertyType != SerializedPropertyType.Enum && !string.IsNullOrEmpty(selectionButtonsAttribute.CollectionName))
			{
				var memberInfo = ReflectionUtility.GetValidMemberInfo(selectionButtonsAttribute.CollectionName, property);
				var displayNames = ConvertCollectionValuesToStrings(selectionButtonsAttribute.CollectionName, property, memberInfo, errorBox).ToArray();

				var buttonsValue = Array.IndexOf(displayNames, GetPropertyValueAsString(property));

				root.Add(DrawButtons(buttonsValue, displayNames, selectionButtonsAttribute, (value) =>
				{
					if (value >= 0 && value < displayNames.Length)
						SetProperyValueFromString(displayNames[value], ref property, errorBox);

					property.serializedObject.ApplyModifiedProperties();
				}));
			}
			else
			{
				errorBox.text = "If the attached field is not an enum, a collection name must be provided";
			}

			DisplayErrorBox(root, errorBox);

			return root;
		}

		private VisualElement DrawButtons(int buttonsValue, string[] valueLabels, SelectionButtonsAttribute selectionButtonsAttribute, Action<int> onValueChanged)
		{
			if (valueLabels == null || valueLabels.Length == 0)
				return new HelpBox("The provided collection is empty", HelpBoxMessageType.Error);

			var toolbarToggles = new Dictionary<ToolbarToggle, int>();
			var toolbar = new OverlayToolbar();

			toolbar.style.flexDirection = FlexDirection.Row;

			if (selectionButtonsAttribute.ShowLabel)
			{
				var label = new Label(preferredLabel);

				label.AddToClassList("unity-base-field__label");

				label.style.flexGrow = 1f;
				label.style.paddingLeft = 4f;
				label.style.minWidth = 100f;

				toolbar.Add(label);
			}
			
			for (int i = 0; i < valueLabels.Length; i++)
			{
				string label = valueLabels[i];
				var toggle = new ToolbarToggle
				{
					text = label,
					value = buttonsValue == i,
					style = {
						flexGrow = 1f,
						height = selectionButtonsAttribute.ButtonsHeight,
					}
				};

				if (i == 0)
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--left");
				}
				else if (i == valueLabels.Length - 1)
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--right");
				}
				else
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--middle");
				}

				// Set default value
				if (i == 0 && buttonsValue == -1)
				{
					toggle.value = true;
					buttonsValue = i;
					onValueChanged.Invoke(buttonsValue);
				}

				toolbar.Add(toggle);
				toolbarToggles.Add(toggle, i);
			}

			foreach (var toggle in toolbarToggles)
			{
				toggle.Key.RegisterValueChangedCallback((callback) =>
				{
					buttonsValue = toggle.Value;

					foreach (var toolbarToggle in toolbarToggles.Where((source) => toggle.Key != source.Key))
					{
						toolbarToggle.Key.SetValueWithoutNotify(false);
					}

					if (buttonsValue == toggle.Value && !toggle.Key.value)
					{
						toggle.Key.SetValueWithoutNotify(true);
					}

					onValueChanged.Invoke(buttonsValue);
				});
			}

			return toolbar;
		}

		private VisualElement DrawEnumFlagButtons(int flagValue, string[] valueLabels, SelectionButtonsAttribute selectionButtonsAttribute, Action<int> onValueChanged)
		{
			var toolbar = new OverlayToolbar();
			toolbar.style.flexDirection = FlexDirection.Row;

			if (selectionButtonsAttribute.ShowLabel)
			{
				var label = new Label(preferredLabel);

				label.style.flexGrow = 1f;
				label.style.minWidth = 100f;

				toolbar.Add(label);
			}

			for (int i = 1; i < valueLabels.Length; i++)
			{
				int enumValue = 1 << i - 1;
				bool isSelected = (flagValue & enumValue) != 0;

				var toggle = new ToolbarToggle() 
				{
					text = valueLabels[i],
					value = isSelected,
					style = {
						flexGrow = 1f,
						height = selectionButtonsAttribute.ButtonsHeight
					}
				};

				if (i == 1)
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--left");
				}
				else if (i == valueLabels.Length - 1)
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--right");
				}
				else
				{
					toggle.AddToClassList("unity-editor-toolbar__button-strip-element--middle");
				}

				toggle.RegisterValueChangedCallback((callback) =>
				{
					if (callback.newValue)
					{
						flagValue |= enumValue;
					}
					else
					{
						flagValue &= ~enumValue;
					}

					onValueChanged.Invoke(flagValue);
				});

				toolbar.Add(toggle);
			}

			return toolbar;
		}
	}
}
