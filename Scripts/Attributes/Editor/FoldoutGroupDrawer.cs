using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FoldoutGroupAttribute))]
    public class FoldoutGroupDrawer : PropertyDrawerBase
    {
		private bool isExpanded;
		private bool hasLoaded;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var foldoutGroup = attribute as FoldoutGroupAttribute;
			var serializedObject = property.serializedObject;

			var isToggledSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_IsToggled";

			if (!hasLoaded)
			{
				isExpanded = EditorPrefs.GetBool(isToggledSaveKey, isExpanded);
				hasLoaded = true;
			}

			isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(isExpanded, foldoutGroup.GroupName);

			EditorGUIUtility.labelWidth = foldoutGroup.LabelWidth;
			EditorGUIUtility.fieldWidth = foldoutGroup.FieldWidth;

			if (isExpanded)
			{
				foreach (string variableName in foldoutGroup.FieldsToGroup)
				{
					var variableProperty = serializedObject.FindProperty(variableName);

					if (variableProperty != null) EditorGUILayout.PropertyField(variableProperty, true);
				}
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			EditorPrefs.SetBool(isToggledSaveKey, isExpanded);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the holder field
	}
}
