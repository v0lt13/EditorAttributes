using UnityEngine;
using UnityEditor;

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
			EditorGUILayout.BeginHorizontal();

			for (int i = 0; i < tabGroupAttribute.FieldsToGroup.Length; i++)
			{
				var currentProperty = property.serializedObject.FindProperty(tabGroupAttribute.FieldsToGroup[i]);
				bool isSelected = i == selectedTab;

				EditorGUI.BeginChangeCheck();
				bool toggleValue = GUILayout.Toggle(isSelected, currentProperty.displayName, EditorStyles.toolbarButton);

				if (EditorGUI.EndChangeCheck() && toggleValue)
					selectedTab = i;
			}

			EditorGUILayout.EndHorizontal();

			var selectedProperty = property.serializedObject.FindProperty(tabGroupAttribute.FieldsToGroup[selectedTab]);

			EditorGUILayout.PropertyField(selectedProperty);

			EditorGUILayout.EndVertical();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;
	}
}
