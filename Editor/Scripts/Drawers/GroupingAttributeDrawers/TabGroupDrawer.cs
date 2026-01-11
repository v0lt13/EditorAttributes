using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TabGroupAttribute))]
    public class TabGroupDrawer : GroupDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var tabGroupAttribute = attribute as TabGroupAttribute;

            string selectedTabSaveKey = CreatePropertySaveKey(property, "SelectedTab");
            string[] propertyNames = GetPropertyDisplayNames(property, tabGroupAttribute);

            TabView tabView = new() { selectedTabIndex = EditorPrefs.GetInt(selectedTabSaveKey) };
            tabView.activeTabChanged += (_, _) => EditorPrefs.SetInt(selectedTabSaveKey, tabView.selectedTabIndex);

            ApplyBoxStyle(tabView);

            for (int i = 0; i < propertyNames.Length; i++)
            {
                string propertyName = propertyNames[i];
                Tab tab = new(propertyName);

                ApplyBoxStyle(tab);

                string fieldName = tabGroupAttribute.FieldsToGroup[i];
                VisualElement groupProperty = CreateGroupProperty(fieldName, property);

                tab.Add(groupProperty);
                tabView.Add(tab);
            }

            return tabView;
        }

        private string[] GetPropertyDisplayNames(SerializedProperty property, TabGroupAttribute tabGroupAttribute)
        {
            List<string> stringList = new();

            foreach (var field in tabGroupAttribute.FieldsToGroup)
            {
                SerializedProperty fieldProperty = FindNestedProperty(property, GetSerializedPropertyName(field, property));

                stringList.Add(fieldProperty == null ? field : fieldProperty.displayName);
            }

            return stringList.ToArray();
        }
    }
}
