using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
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

			var selectedTabSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_SelectedTab";

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

			var propertyField = GetDrawnProperty(property, tabGroupAttribute);

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

					propertyField = GetDrawnProperty(property, tabGroupAttribute);
					propertyField.style.marginLeft = 10f;

					root.Add(propertyField);
				});
			}

			root.Add(toolbar);
			root.Add(propertyField);

			return root;
		}

		private VisualElement GetDrawnProperty(SerializedProperty property, TabGroupAttribute tabGroupAttribute)
		{
			var selectedProperty = FindNestedProperty(property, GetSerializedPropertyName(tabGroupAttribute.FieldsToGroup[selectedTab], property));

			var propertyField = DrawProperty(selectedProperty);

			return propertyField;
		}

		private string[] GetPropertyNames(SerializedProperty property, TabGroupAttribute tabGroupAttribute)
		{
			var stringList = new List<string>();

			foreach (var field in tabGroupAttribute.FieldsToGroup)
			{
				var fieldProperty = FindNestedProperty(property, GetSerializedPropertyName(field, property));

				stringList.Add(fieldProperty.displayName);
			}

			return stringList.ToArray();
		}
	}
}
