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
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(enableAttribute.ConditionName, property);

			using (new EditorGUI.DisabledGroupScope(!GetConditionValue(conditionalProperty, enableAttribute, property)))
				DrawProperty(position, property, label);
		}
	}
}
