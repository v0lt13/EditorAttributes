using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideInChildrenAttribute))]
    public class HideInChildrenDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
			if (!IsPropertyInherited(property))
			    DrawProperty(position, property, label);
    	}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!IsPropertyInherited(property))
				return base.GetPropertyHeight(property, label);

			return -EditorGUIUtility.standardVerticalSpacing;
		}

		private bool IsPropertyInherited(SerializedProperty property)
		{
			var targetObjectType = property.serializedObject.targetObject.GetType();
			var fieldInfo = targetObjectType.GetField(property.name, ReflectionUtility.BINDING_FLAGS);

			return fieldInfo == null; // If fieldInfo is null it means that the field was not found in the target, so it must be inherited
		}
	}
}
