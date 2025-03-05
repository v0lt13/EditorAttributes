using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(FoldoutGroupAttribute))]
    public class FoldoutGroupDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var foldoutGroup = attribute as FoldoutGroupAttribute;
			var isFoldedSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_IsFolded";

			var root = new VisualElement();

			var foldout = new Foldout
			{
				style = { unityFontStyleAndWeight = FontStyle.Bold },
				text = foldoutGroup.GroupName,
				tooltip = property.tooltip,
				value = EditorPrefs.GetBool(isFoldedSaveKey)
			};

			if (foldoutGroup.DrawInBox)
				ApplyBoxStyle(foldout.contentContainer);

			foreach (string variableName in foldoutGroup.FieldsToGroup)
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
							label.style.marginRight = foldoutGroup.WidthOffset;
					});
				}
				else
				{
					foldout.Add(new HelpBox($"{variableName} is not a valid field", HelpBoxMessageType.Error));
					break;
				}
			}

			foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(isFoldedSaveKey, callback.newValue));
			root.Add(foldout);

			ExecuteLater(foldout, () =>
			{
				var toggle = foldout.Q<Toggle>();

				toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);
			});

			return root;
		}
	}
}
