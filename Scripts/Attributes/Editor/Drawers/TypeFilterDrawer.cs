using System;
using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TypeFilterAttribute))]
    public class TypeFilterDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var typeFilterAttribute = attribute as TypeFilterAttribute;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                var oldObjectReferenceValue = property.objectReferenceValue;

				EditorGUI.ObjectField(position, property, label);

                if (property.objectReferenceValue != null)
                {
                    foreach (var type in typeFilterAttribute.TypesToFilter)
                    {
						var filterType = (Type)type;

                        if (filterType.IsAssignableFrom(property.objectReferenceValue.GetType())) return;
					}

                    property.objectReferenceValue = oldObjectReferenceValue;
                }
			}
            else
            {
				EditorGUILayout.HelpBox($"The attached field must be an object", MessageType.Error);
			}
    	}
    }
}
