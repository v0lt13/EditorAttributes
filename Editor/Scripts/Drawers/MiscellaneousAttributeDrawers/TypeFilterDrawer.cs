using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TypeFilterAttribute))]
    public class TypeFilterDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return new HelpBox("The attached field must derive from <b>UnityEngine.Object</b>", HelpBoxMessageType.Error);

            var typeFilterAttribute = attribute as TypeFilterAttribute;

            PropertyField propertyField = CreatePropertyField(property);

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var objectField = propertyField.Q<ObjectField>();

                objectField.objectType = property.objectReferenceValue != null ? property.objectReferenceValue.GetType() : typeFilterAttribute.TypesToFilter.FirstOrDefault();

                objectField.RegisterCallback<DragEnterEvent>(callback =>
                {
                    Object draggedObject = DragAndDrop.objectReferences.FirstOrDefault();

                    if (draggedObject != null)
                    {
                        Type acceptedDraggedType = null;

                        // Check if the dragged object is compatible with any of the allowed types
                        bool isValidType = typeFilterAttribute.TypesToFilter.Any(type =>
                        {
                            var objectType = type;

                            if (objectType.IsInstanceOfType(draggedObject))
                            {
                                acceptedDraggedType = objectType;
                                return true;
                            }

                            return false;
                        });

                        if (isValidType)
                            objectField.objectType = acceptedDraggedType;
                    }
                });

                objectField.TrackPropertyValue(property, (trackedProperty) => objectField.objectType = trackedProperty.objectReferenceValue != null ? trackedProperty.objectReferenceValue.GetType() : typeFilterAttribute.TypesToFilter.FirstOrDefault());
            });

            return propertyField;
        }
    }
}
