using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(CollectionRangeAttribute))]
    public class CollectionRangeDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!property.isArray)
                return new HelpBox("The CollectionRange Attribute can only be used on collections", HelpBoxMessageType.Error);

            var collectionRangeAttribute = attribute as CollectionRangeAttribute;

            PropertyField propertyField = CreatePropertyField(property);

            ClampCollectionSize(property, collectionRangeAttribute);

            propertyField.RegisterValueChangeCallback((callback) => ClampCollectionSize(property, collectionRangeAttribute));

            return propertyField;
        }

        private void ClampCollectionSize(SerializedProperty property, CollectionRangeAttribute collectionRangeAttribute)
        {
            property.arraySize = Mathf.Clamp(property.arraySize, collectionRangeAttribute.MinRange, collectionRangeAttribute.MaxRange);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
