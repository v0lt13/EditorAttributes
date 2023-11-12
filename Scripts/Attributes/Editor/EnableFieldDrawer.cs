using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(EnableFieldAttribute))]
    public class EnableFieldDrawer : PropertyDrawerBase
    {
		private MemberInfo conditionalProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var enableAttribute = attribute as EnableFieldAttribute;

			conditionalProperty = GetValidMemberInfo(enableAttribute.ConditionName, property);

			GUI.enabled = GetConditionValue<EnableFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject, true);
			
			DrawProperty(position, property, label);

			GUI.enabled = true;
		}
	}
}
