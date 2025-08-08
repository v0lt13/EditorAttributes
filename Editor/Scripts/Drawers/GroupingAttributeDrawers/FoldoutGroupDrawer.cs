using UnityEditor;
using UnityEngine;
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
			var foldoutSaveKey = CreatePropertySaveKey(property, "IsFoldoutGroupFolded");

			var root = new VisualElement();

			var foldout = new Foldout
			{
				style = { unityFontStyleAndWeight = FontStyle.Bold },
				text = foldoutGroup.GroupName,
				tooltip = property.tooltip,
				value = EditorPrefs.GetBool(foldoutSaveKey)
			};

			if (foldoutGroup.DrawInBox)
				ApplyBoxStyle(foldout.contentContainer);

			foreach (string variableName in foldoutGroup.FieldsToGroup)
			{
				var propertyField = CreateField(variableName, property, root);

				foldout.Add(propertyField);
			}

			root.Add(foldout);

			ExecuteLater(foldout, () =>
			{
				var toggle = foldout.Q<Toggle>();

				toggle.style.backgroundColor = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR / 3f : new Color(0.1f, 0.1f, 0.1f, 0.2f);

				// Register this callback later since value changed callbacks are called on inspector initalization and we don't want to save values on initalization
				foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));
			});

			return root;
		}

		private VisualElement CreateField(string variableName, SerializedProperty property, VisualElement root)
		{
			VisualElement field;

			var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

			if (variableProperty == null)
				return new HelpBox($"<b>{variableName}</b> is not a valid field or property", HelpBoxMessageType.Error);

			field = CreatePropertyField(variableProperty);

			field.style.unityFontStyleAndWeight = FontStyle.Normal;

			// Slightly move foldouts for serialized objects
			if (variableProperty.propertyType == SerializedPropertyType.Generic && variableProperty.type != "UnityEvent" && !ReflectionUtility.IsPropertyCollection(variableProperty))
				field.style.marginLeft = 10f;

			root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			void OnGeometryChanged(GeometryChangedEvent changeEvent)
			{
				// Force update this logic to make sure fields are visible
				UpdateVisualElement(field, () =>
				{
					var hiddenField = field.Q<VisualElement>(HidePropertyDrawer.HIDDEN_PROPERTY_ID);

					if (hiddenField != null)
					{
						hiddenField.name = GROUPED_PROPERTY_ID;
						hiddenField.style.display = DisplayStyle.Flex;
					}
				}, 100L).ForDuration(400L);

				root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			return field;
		}
	}
}
