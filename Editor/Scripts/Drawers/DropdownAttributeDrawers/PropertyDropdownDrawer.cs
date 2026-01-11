using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PropertyDropdownAttribute))]
    public class PropertyDropdownDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return new HelpBox("The PropertyDropdown Attribute can only be attached on to <b>UnityEngine.Object</b> types", HelpBoxMessageType.Error);

            VisualElement root = new();
            PropertyField propertyField = CreatePropertyField(property);

            ApplyBoxStyle(root);

            root.Add(propertyField);

            InitializeFoldoutDrawer(root, property);

            propertyField.RegisterCallback<SerializedPropertyChangeEvent>((callback) =>
            {
                if (root.childCount > 1 && root.ElementAt(1) != null)
                    root.RemoveAt(1);

                InitializeFoldoutDrawer(root, property);
            });

            return root;
        }

        private void InitializeFoldoutDrawer(VisualElement root, SerializedProperty property)
        {
            if (property.objectReferenceValue == null)
            {
                var foldout = root.Q<Foldout>();

                if (foldout != null)
                    root.Remove(foldout);

                return;
            }

            root.Add(CreatePropertyFoldout(new SerializedObject(property.objectReferenceValue), property));
        }

        private Foldout CreatePropertyFoldout(SerializedObject serializedObject, SerializedProperty serializedProperty)
        {
            string foldoutSaveKey = CreatePropertySaveKey(serializedProperty, "IsPropertyDropdownFolded");

            Foldout foldout = new()
            {
                text = "Properties",
                value = EditorPrefs.GetBool(foldoutSaveKey)
            };

            ApplyBoxStyle(foldout);

            foldout.style.paddingLeft = 15f;
            foldout.Add(new InspectorElement(serializedObject));

            serializedObject.ApplyModifiedProperties();

            foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));
            foldout.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                foldout.Q<Label>(className: Foldout.textUssClassName).style.unityFontStyleAndWeight = FontStyle.Bold;
                foldout.Q<ObjectField>("unity-input-m_Script")?.parent.RemoveFromHierarchy();
            });

            return foldout;
        }
    }
}
