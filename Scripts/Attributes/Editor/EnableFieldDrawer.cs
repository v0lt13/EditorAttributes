using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(EnableFieldAttribute))]
    public class EnableFieldDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var enableAttribute = attribute as EnableFieldAttribute;

			var conditionalProperty = property.serializedObject.FindProperty(enableAttribute.conditionName);

			if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
			{
				bool conditionalValue = conditionalProperty.boolValue;

				GUI.enabled = conditionalValue;
				EditorGUI.PropertyField(position, property, label);
				GUI.enabled = true;
			}
			else
			{
				EditorGUILayout.HelpBox($"The provided condition \"{enableAttribute.conditionName}\" is not a valid boolean", MessageType.Warning);
			}
		}
	}
}
