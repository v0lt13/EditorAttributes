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

                EditorGUI.BeginChangeCheck();
				EditorGUI.ObjectField(position, property, label);

                if (property.objectReferenceValue != null && EditorGUI.EndChangeCheck())
                {
                    foreach (var type in typeFilterAttribute.TypesToFilter)
                    {
						var filterType = (Type)type;

                        // If the object value is of type Transform and we do not filter by that type that means the user just dragged a gameObject into the field so we try to see if that gameObject contains the type we filter by
                        if (property.objectReferenceValue.GetType() == typeof(Transform) && filterType != typeof(Transform))
                        {
                            var objectComponent = (Component)property.objectReferenceValue;

                            if (objectComponent.TryGetComponent(filterType, out Component component))
                            {
                                property.objectReferenceValue = component;
                                return;
                            }
                        }
                        else if (filterType.IsAssignableFrom(property.objectReferenceValue.GetType()))
                        {
                            return;
                        }
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
