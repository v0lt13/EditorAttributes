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

				toggle.style.backgroundColor = canApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

				var parentElement = foldout.Q<Label>().parent;

				parentElement.Insert(1, toggleBox);
			}).ExecuteLater(1);

			root.Add(toggleBox);

			foreach (string variableName in toggleGroup.FieldsToGroup)
			{
				var variableProperty = FindNestedProperty(property, variableName);

				// Check for serialized properties since they have a weird naming when serialized and they cannot be found by the normal name
				variableProperty ??= FindNestedProperty(property, $"<{variableName}>k__BackingField");

				if (variableProperty != null)
				{
					var properyField = DrawProperty(variableProperty);

					properyField.style.unityFontStyleAndWeight = FontStyle.Normal;
					properyField.schedule.Execute(() => properyField.Q<Label>().style.marginRight = toggleGroup.WidthOffset).ExecuteLater(1);

					foldout.Add(properyField);
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
