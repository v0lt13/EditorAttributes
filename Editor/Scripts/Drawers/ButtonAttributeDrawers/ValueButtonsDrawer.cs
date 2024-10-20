using UnityEditor;
using UnityEngine.UIElements;

#if UNITY_6000_0_OR_NEWER
using System;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
#endif

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ValueButtonsAttribute))]
	public class ValueButtonsDrawer : PropertyDrawerBase
	{
#if !UNITY_6000_0_OR_NEWER
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			root.Add(new HelpBox("This attribute is only available in <b>Unity 6 and above</b>, use the <b>SelectionButtons Attribute</b> for the same functionality", HelpBoxMessageType.Warning));

			return root;
		}
#else
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var selectionButtonsAttribute = attribute as ValueButtonsAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();
			
			var memberInfo = ReflectionUtility.GetValidMemberInfo(selectionButtonsAttribute.CollectionName, property);
			var displayNames = ConvertCollectionValuesToStrings(selectionButtonsAttribute.CollectionName, property, memberInfo, errorBox).ToArray();

			var buttonsValue = Array.IndexOf(displayNames, GetPropertyValueAsString(property));

			root.Add(DrawButtons(buttonsValue, displayNames, selectionButtonsAttribute, (value) =>
			{
				if (value >= 0 && value < displayNames.Length)
					SetProperyValueFromString(displayNames[value], ref property, errorBox);

				property.serializedObject.ApplyModifiedProperties();
			}));

			DisplayErrorBox(root, errorBox);

			return root;
		}

		private VisualElement DrawButtons(int buttonsValue, string[] valueLabels, ValueButtonsAttribute selectionButtonsAttribute, Action<int> onValueChanged)
		{
			if (valueLabels == null || valueLabels.Length == 0)
				return new HelpBox("The provided collection is empty", HelpBoxMessageType.Error);

			var activeButtonList = new List<bool>();
			var buttonGroup = new ToggleButtonGroup(selectionButtonsAttribute.ShowLabel ? preferredLabel : string.Empty);
			
			foreach (string label in valueLabels)
			{
				var toggle = new Button
				{
					text = label,
					style = { height = selectionButtonsAttribute.ButtonsHeight }
				};

				activeButtonList.Add(false);
				buttonGroup.Add(toggle);
			}

			activeButtonList[buttonsValue == -1 ? 0 : buttonsValue] = true;

			buttonGroup.SetValueWithoutNotify(ToggleButtonGroupState.CreateFromOptions(activeButtonList));
			buttonGroup.RegisterValueChangedCallback((value) => onValueChanged.Invoke(value.newValue.GetActiveOptions(ConvertBoolsToSpan(activeButtonList))[0]));

			return buttonGroup;
		}

		private static Span<int> ConvertBoolsToSpan(List<bool> boolList)
		{
			var intArray = new int[boolList.Count];

			for (int i = 0; i < boolList.Count; i++)
				intArray[i] = boolList[i] ? 1 : 0;

			return new Span<int>(intArray);
		}
#endif
	}
}
