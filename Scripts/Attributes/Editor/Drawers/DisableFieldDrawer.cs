using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var disableAttribute = attribute as DisableFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(disableAttribute.ConditionName, property);

			GUI.enabled = !GetConditionValue(conditionalProperty, disableAttribute, property);

			DrawProperty(position, property, label);

			GUI.enabled = true;
		}
	}
}
