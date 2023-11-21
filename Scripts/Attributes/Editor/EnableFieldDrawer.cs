using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(EnableFieldAttribute))]
    public class EnableFieldDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var enableAttribute = attribute as EnableFieldAttribute;
			var conditionalProperty = GetValidMemberInfo(enableAttribute.ConditionName, property);

			GUI.enabled = GetConditionValue<EnableFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject);
			
			DrawProperty(position, property, label);

			GUI.enabled = true;
		}
	}
}
