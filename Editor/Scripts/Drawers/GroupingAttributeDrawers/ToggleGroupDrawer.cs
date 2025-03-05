using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EditorAttributes.Editor.Utility;

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
				tooltip = property.tooltip,
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

			root.Add(toggleBox);

			foreach (string variableName in toggleGroup.FieldsToGroup)
			{
				var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

				if (variableProperty != null)
				{
					var propertyField = new PropertyField(variableProperty);

					// Slightly move foldouts for serialized objects
					if (variableProperty.propertyType == SerializedPropertyType.Generic && variableProperty.type != "UnityEvent" && !ReflectionUtility.IsPropertyCollection(variableProperty))
						propertyField.style.marginLeft = 10f;

					propertyField.style.unityFontStyleAndWeight = FontStyle.Normal;

					foldout.Add(propertyField);

					ExecuteLater(propertyField, () =>
					{
						var label = propertyField.Q<Label>();

						if (label != null)
							label.style.marginRight = toggleGroup.WidthOffset;
					});
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

			ExecuteLater(foldout, () =>
			{
				var toggle = foldout.Q<Toggle>();

				toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

				var parentElement = foldout.Q<Label>().parent;

				parentElement.Insert(1, toggleBox);
			});

			return root;
		}
	}
}
