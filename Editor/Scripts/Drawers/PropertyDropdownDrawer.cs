using System;
using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PropertyDropdownAttribute))]
    public class PropertyDropdownDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var currentField = ReflectionUtility.GetValidMemberInfo(property.name, property);
            var fieldType = ReflectionUtility.GetMemberInfoType(currentField);

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			EditorGUILayout.PropertyField(property);

		    if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
				if (property.objectReferenceValue == null)
				{
					EditorGUILayout.EndVertical();
					return;
				}

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				

				var foldoutStyle = new GUIStyle(EditorStyles.foldout) 
				{
					margin = new RectOffset(8, 0, 0, 0),
					fontStyle = FontStyle.Bold
				};

				property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, "Properties", foldoutStyle);

				using (new EditorGUI.IndentLevelScope(1))
				{
					if (fieldType.IsSubclassOf(typeof(Component)) || fieldType == typeof(Component))
					{
						var component = property.objectReferenceValue as Component;

						if (property.isExpanded) DrawProperties(new SerializedObject(component));
					}
					else if (fieldType.IsSubclassOf(typeof(ScriptableObject)) || fieldType == typeof(ScriptableObject))
					{
						var scriptableObject = property.objectReferenceValue as ScriptableObject;

						if (property.isExpanded) DrawProperties(new SerializedObject(scriptableObject));
					}
					else
					{
						EditorGUILayout.HelpBox("The PropertyDropdown Attribute can only be attached to objects deriving from Component or ScriptableObject", MessageType.Error);
					}
				}

				EditorGUILayout.EndVertical();
			}
            else
            {
				EditorGUILayout.HelpBox("The PropertyDropdown Attribute can only be attached to objects deriving from Component or ScriptableObject", MessageType.Error);
			}

			EditorGUILayout.EndVertical();
    	}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;

		private void DrawProperties(SerializedObject serializedObject)
        {
            serializedObject.Update();

			using (var iterator = serializedObject.GetIterator())
			{
				if (iterator.NextVisible(true))
				{
					do
					{
						var childProperty = serializedObject.FindProperty(iterator.name);

						if (childProperty.name.Equals("m_Script", StringComparison.Ordinal)) continue; // Exclude the field containing the script reference

						EditorGUILayout.PropertyField(childProperty, true);
					}
					while (iterator.NextVisible(false));
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
    }
}
