using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
				EditorGUI.ObjectField(position, property, label);

                if (property.objectReferenceValue == null) EditorGUILayout.HelpBox($"The field \"{property.displayName}\" must be assigned", MessageType.Error);
            }
            else
            {
                DrawProperty(position, property, label);
				EditorGUILayout.HelpBox($"The attached field must be an object", MessageType.Error);
			}
    	}
    }
}
