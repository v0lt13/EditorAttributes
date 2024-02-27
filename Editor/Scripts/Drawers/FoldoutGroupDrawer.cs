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
			var isToggledSaveKey = $"{property.serializedObject.targetObject}_{property.propertyPath}_IsToggled";

			if (!hasLoaded)
			{
				isExpanded = EditorPrefs.GetBool(isToggledSaveKey, isExpanded);
				hasLoaded = true;
			}

			isExpanded = DrawFoldout(foldoutGroup.GroupName, isExpanded);

			EditorGUIUtility.labelWidth = foldoutGroup.LabelWidth;
			EditorGUIUtility.fieldWidth = foldoutGroup.FieldWidth;

			if (isExpanded)
			{
				var groupStyle = foldoutGroup.DrawInBox ? EditorStyles.helpBox : EditorStyles.inspectorFullWidthMargins;

				EditorGUILayout.BeginVertical(groupStyle);

				foreach (string variableName in foldoutGroup.FieldsToGroup)
				{
					var variableProperty = FindNestedProperty(property, variableName);

					// Check for serialized properties since they have a weird naming when serialized and they cannot be found by the normal name
					variableProperty ??= FindNestedProperty(property, $"<{variableName}>k__BackingField");

					if (variableProperty != null) 
					{
						EditorGUILayout.PropertyField(variableProperty, true);
					}
					else
					{
						EditorGUILayout.HelpBox($"{variableName} is not a valid field", MessageType.Error);
						break;
					}
				}

				EditorGUILayout.EndVertical();
			}

			EditorPrefs.SetBool(isToggledSaveKey, isExpanded);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the holder field

		private bool DrawFoldout(string title, bool isExpanded)
		{
			var backgroundRect = GUILayoutUtility.GetRect(1f, 17f);

			var labelRect = backgroundRect;
			labelRect.xMin += 15f;
			labelRect.xMax -= 20f;

			var foldoutRect = backgroundRect;
			foldoutRect.y += 1f;
			foldoutRect.width = 13f;
			foldoutRect.height = 13f;

			var toggleRect = backgroundRect;
			toggleRect.x += 16f;
			toggleRect.y += 2f;
			toggleRect.width = 13f;
			toggleRect.height = 13f;
			backgroundRect.width += 4f;

			EditorGUI.DrawRect(backgroundRect, new Color(0.1f, 0.1f, 0.1f, 0.2f));

			EditorGUI.LabelField(labelRect, title, EditorStyles.boldLabel);
			isExpanded = EditorGUI.Toggle(foldoutRect, isExpanded, EditorStyles.foldout);

			var @event = Event.current;

			if (@event.type == EventType.MouseDown && labelRect.Contains(@event.mousePosition) && @event.button == 0)
			{
				isExpanded = !isExpanded;
				@event.Use();
			}

			return isExpanded;
		}
	}
}
