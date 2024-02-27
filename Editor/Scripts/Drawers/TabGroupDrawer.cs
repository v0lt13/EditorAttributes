using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TabGroupAttribute))]
    public class TabGroupDrawer : PropertyDrawerBase
    {
		private int selectedTab = 0;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var tabGroupAttribute = attribute as TabGroupAttribute;

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			selectedTab = GUILayout.Toolbar(selectedTab, GetPropertyNames(property, tabGroupAttribute), EditorStyles.toolbarButton);

			var selectedProperty = FindNestedProperty(property, tabGroupAttribute.FieldsToGroup[selectedTab]);

			// Check for serialized properties since they have a weird naming when serialized and they cannot be found by the normal name
			selectedProperty ??= FindNestedProperty(property, $"<{tabGroupAttribute.FieldsToGroup[selectedTab]}>k__BackingField");

			EditorGUILayout.PropertyField(selectedProperty);

			EditorGUILayout.EndVertical();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;

		private string[] GetPropertyNames(SerializedProperty property, TabGroupAttribute tabGroupAttribute)
		{
			var stringList = new List<string>();

			foreach (var field in tabGroupAttribute.FieldsToGroup)
			{
				var fieldProperty = FindNestedProperty(property, field);
				fieldProperty ??= FindNestedProperty(property, $"<{field}>k__BackingField");

				stringList.Add(fieldProperty.displayName);
			}

			return stringList.ToArray();
		}
	}
}
