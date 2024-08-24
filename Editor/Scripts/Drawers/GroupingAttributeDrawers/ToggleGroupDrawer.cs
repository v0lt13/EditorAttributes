using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ToggleGroupAttribute))]
    public class ToggleGroupDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var toggleGroup = attribute as ToggleGroupAttribute;
			var isFoldedSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_IsFolded";
			var isToggledSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_IsToggled";

			var root = new VisualElement();

			var foldout = new Foldout
			{
				text = toggleGroup.GroupName,
				style = { unityFontStyleAndWeight = FontStyle.Bold },
				value = EditorPrefs.GetBool(isFoldedSaveKey)
			};

			var toggleBox = new Toggle()
			{
				text = "",
				style = { marginRight = 10f },
				value = EditorPrefs.GetBool(isToggledSaveKey)
			};

			foldout.contentContainer.SetEnabled(toggleBox.value);

			if (toggleGroup.DrawInBox)
				ApplyBoxStyle(foldout.contentContainer);

			foldout.schedule.Execute(() =>
			{
				var toggle = foldout.Q<Toggle>();

				toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

				var parentElement = foldout.Q<Label>().parent;

				parentElement.Insert(1, toggleBox);
			}).ExecuteLater(1);

			root.Add(toggleBox);

			foreach (string variableName in toggleGroup.FieldsToGroup)
			{
				var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

				if (variableProperty != null)
				{
					var propertyField = DrawProperty(variableProperty);

					if (variableProperty.propertyType == SerializedPropertyType.Generic) // Slightly move dropdowns for serialized objects
						propertyField.style.marginLeft = 10f;

					propertyField.style.unityFontStyleAndWeight = FontStyle.Normal;
					propertyField.schedule.Execute(() => propertyField.Q<Label>().style.marginRight = toggleGroup.WidthOffset).ExecuteLater(1);

					foldout.Add(propertyField);
				}
				else
				{
					foldout.Add(new HelpBox($"{variableName} is not a valid field", HelpBoxMessageType.Error));
					break;
				}
			}

			foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(isFoldedSaveKey, callback.newValue));
			toggleBox.RegisterValueChangedCallback((callback) =>
			{
				if (property.propertyType == SerializedPropertyType.Boolean)
				{
					property.boolValue = callback.newValue;
					property.serializedObject.ApplyModifiedProperties();
				}

				foldout.contentContainer.SetEnabled(callback.newValue);

				EditorPrefs.SetBool(isFoldedSaveKey, foldout.value);
				EditorPrefs.SetBool(isToggledSaveKey, callback.newValue);
			});

			root.Add(foldout);

			return root;
		}
	}
}
