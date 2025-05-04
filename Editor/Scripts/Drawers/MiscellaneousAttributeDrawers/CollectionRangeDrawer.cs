using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(CollectionRangeAttribute))]
    public class CollectionRangeDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var collectionRangeAttribute = attribute as CollectionRangeAttribute;
            var root = new VisualElement();

            if (!ReflectionUtility.IsPropertyCollection(property))
                return new HelpBox("The CollectionRange Attribute can only be used on collections", HelpBoxMessageType.Error);

            var propertyField = CreatePropertyField(property);

#if UNITY_2023_3_OR_NEWER
            ClampCollectionSize(property, collectionRangeAttribute);

            propertyField.RegisterValueChangeCallback((evt) => ClampCollectionSize(property, collectionRangeAttribute));
#else
            root.Add(new HelpBox("The CollectionRange Attribute is only available in <b>Unity 6 and above</b>", HelpBoxMessageType.Warning));
#endif

            root.Add(propertyField);

            return root;
        }

        private void ClampCollectionSize(SerializedProperty property, CollectionRangeAttribute collectionRangeAttribute)
        {
            property.arraySize = Mathf.Clamp(property.arraySize, collectionRangeAttribute.MinRange, collectionRangeAttribute.MaxRange);
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
