using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TabGroupAttribute))]
	public class TabGroupDrawer : PropertyDrawerBase
	{
		private int selectedTab = 0;
		private Dictionary<ToolbarToggle, int> toolbarToggles = new();

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var tabGroupAttribute = attribute as TabGroupAttribute;
			var root = new VisualElement();

			var selectedTabSaveKey = CreatePropertySaveKey(property, "SelectedTab");

			selectedTab = EditorPrefs.GetInt(selectedTabSaveKey);

			ApplyBoxStyle(root);

			var toolbar = new Toolbar();

			var propertyNames = GetPropertyNames(property, tabGroupAttribute);

			for (int i = 0; i < propertyNames.Length; i++)
			{
				string propertyName = propertyNames[i];
				var toolbarToggle = new ToolbarToggle()
				{
					text = propertyName,
					value = selectedTab == i,
					style = {
						flexGrow = 1f,
						unityFontStyleAndWeight = FontStyle.Bold,
					}
				};

				toolbarToggles.Add(toolbarToggle, i);
				toolbar.Add(toolbarToggle);
			}

			var propertyField = CreateField(tabGroupAttribute.FieldsToGroup[selectedTab], property);

			propertyField.style.marginLeft = 10f;

			foreach (var toggle in toolbarToggles)
			{
				toggle.Key.RegisterValueChangedCallback((callback) =>
				{
					selectedTab = toggle.Value;

					EditorPrefs.SetInt(selectedTabSaveKey, selectedTab);

					foreach (var toolbarToggle in toolbarToggles.Where((source) => toggle.Key != source.Key))
						toolbarToggle.Key.SetValueWithoutNotify(false);

					if (selectedTab == toggle.Value && !toggle.Key.value)
						toggle.Key.SetValueWithoutNotify(true);

					root.Remove(propertyField);

					propertyField = CreateField(tabGroupAttribute.FieldsToGroup[selectedTab], property);
					propertyField.style.marginLeft = 10f;

					root.Add(propertyField);
				});
			}

			root.Add(toolbar);
			root.Add(propertyField);

			return root;
		}

		private VisualElement CreateField(string variableName, SerializedProperty property)
		{
			VisualElement field;

			var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

			if (variableProperty == null)
				return new HelpBox($"<b>{variableName}</b> is not a valid field or property", HelpBoxMessageType.Error);

			field = CreatePropertyField(variableProperty);

			// Replace the hidden field ID with the grouped field ID so the EditorExtension class doesn't remove it when drawing the editor
			field.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

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

				field.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			return field;
		}

		private string[] GetPropertyNames(SerializedProperty property, TabGroupAttribute tabGroupAttribute)
		{
			var stringList = new List<string>();

			foreach (var field in tabGroupAttribute.FieldsToGroup)
			{
				var fieldProperty = FindNestedProperty(property, GetSerializedPropertyName(field, property));

				stringList.Add(fieldProperty == null ? field : fieldProperty.displayName);
			}

			return stringList.ToArray();
		}
	}
}
