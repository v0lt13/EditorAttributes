using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ToggleGroupAttribute))]
	public class ToggleGroupDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var toggleGroup = attribute as ToggleGroupAttribute;
			var foldoutSaveKey = CreatePropertySaveKey(property, "IsToggleGroupFolded");
			var toggleSaveKey = CreatePropertySaveKey(property, "IsToggleGroupToggled");

			var root = new VisualElement();

			var foldout = new Foldout
			{
				text = toggleGroup.GroupName,
				tooltip = property.tooltip,
				style = { unityFontStyleAndWeight = FontStyle.Bold },
				value = EditorPrefs.GetBool(foldoutSaveKey)
			};

			var toggleBox = new Toggle()
			{
				text = "",
				style = { marginRight = 10f },
				value = property.propertyType == SerializedPropertyType.Boolean ? property.boolValue : EditorPrefs.GetBool(toggleSaveKey)
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
					var propertyField = CreatePropertyField(variableProperty);

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

			toggleBox.RegisterValueChangedCallback((callback) =>
			{
				if (property.propertyType == SerializedPropertyType.Boolean)
				{
					property.boolValue = callback.newValue;
					property.serializedObject.ApplyModifiedProperties();
				}
				else
				{
					EditorPrefs.SetBool(toggleSaveKey, callback.newValue); // The value is already serialized via the property, there is no point in saving it.
				}

				foldout.contentContainer.SetEnabled(callback.newValue);
			});

			root.Add(foldout);

			ExecuteLater(foldout, () =>
			{
				var toggle = foldout.Q<Toggle>();

				toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

				var parentElement = foldout.Q<Label>().parent;

				parentElement.Insert(1, toggleBox);

				// Register this callback later since value changed callbacks are called on inspector initalization and we don't want to save values on initalization
				foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));
			});

			if (property.propertyType == SerializedPropertyType.Boolean)
				UpdateVisualElement(toggleBox, () => toggleBox.value = property.boolValue);

			return root;
		}
	}
}
