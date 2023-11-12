using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawerBase
    {
		private MemberInfo conditionalProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var disableAttribute = attribute as DisableFieldAttribute;

			conditionalProperty = GetValidMemberInfo(disableAttribute.ConditionName, property);

			GUI.enabled = !GetConditionValue<DisableFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject, true);

			DrawProperty(position, property, label);

			GUI.enabled = true;
		}
	}
}
