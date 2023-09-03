using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var disableAttribute = attribute as DisableFieldAttribute;

			var conditionalProperty = property.serializedObject.FindProperty(disableAttribute.conditionName);

			if (conditionalProperty != null && conditionalProperty.propertyType == SerializedPropertyType.Boolean)
			{
				bool conditionalValue = conditionalProperty.boolValue;

				GUI.enabled = !conditionalValue;
				EditorGUI.PropertyField(position, property, label);
				GUI.enabled = true;
			}
			else
			{
				EditorGUILayout.HelpBox($"The provided condition \"{disableAttribute.conditionName}\" is not a valid boolean", MessageType.Warning);
			}
		}
	}
}
