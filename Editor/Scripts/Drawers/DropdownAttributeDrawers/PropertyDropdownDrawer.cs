using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(PropertyDropdownAttribute))]
    public class PropertyDropdownDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var currentField = ReflectionUtility.GetValidMemberInfo(property.name, property);
			var fieldType = ReflectionUtility.GetMemberInfoType(currentField);

			var root = new VisualElement();
			var propertyField = DrawProperty(property);
			var errorBox = new HelpBox("The PropertyDropdown Attribute can only be attached to objects deriving from Component or ScriptableObject", HelpBoxMessageType.Error);

			ApplyBoxStyle(root);

			root.Add(propertyField);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				// Register the callback later to skip any initialization calls
				root.schedule.Execute(() =>
				{
					propertyField.RegisterCallback<SerializedPropertyChangeEvent>((callback) =>
					{
						if (root.childCount > 1 && root.ElementAt(1) != null)
							root.RemoveAt(1);

						InitializeFoldoutDrawer(root, property, fieldType, errorBox);
					});
				}).ExecuteLater(1);

				InitializeFoldoutDrawer(root, property, fieldType, errorBox);
			}
			else
			{
				root.Add(errorBox);
			}

			return root;
		}

		private void InitializeFoldoutDrawer(VisualElement root, SerializedProperty property, Type fieldType, HelpBox errorBox)
		{
			if (property.objectReferenceValue == null)
			{
				var foldout = root.Q<Foldout>();

				if (foldout != null)
					root.Remove(foldout);

				return;
			}

			if (fieldType.IsSubclassOf(typeof(Component)) || fieldType == typeof(Component))
			{
				var component = property.objectReferenceValue as Component;

				root.Add(CreatePropertyFoldout(new SerializedObject(component), property));
			}
			else if (fieldType.IsSubclassOf(typeof(ScriptableObject)) || fieldType == typeof(ScriptableObject))
			{
				var scriptableObject = property.objectReferenceValue as ScriptableObject;

				root.Add(CreatePropertyFoldout(new SerializedObject(scriptableObject), property));
			}
			else
			{
				root.Add(errorBox);
			}
		}

		private Foldout CreatePropertyFoldout(SerializedObject serializedObject, SerializedProperty serilizedProperty)
        {
			var isFoldedSaveKey = $"{serilizedProperty.serializedObject.targetObject}_{serilizedProperty.propertyPath}_IsFolded";

			var foldout = new Foldout 
			{
				text = "Properties",
				value = EditorPrefs.GetBool(isFoldedSaveKey)
			};

			ApplyBoxStyle(foldout);

			foldout.style.unityFontStyleAndWeight = FontStyle.Bold;
			foldout.style.paddingLeft = 15f;

			using (var property = serializedObject.GetIterator())
			{
				if (property.NextVisible(true))
				{
					do
					{
						if (property.name.Equals("m_Script", StringComparison.Ordinal)) // Exclude the field containing the script reference
							continue; 

						var propertyField = DrawProperty(property);

						propertyField.style.unityFontStyleAndWeight = FontStyle.Normal;

						foldout.Add(propertyField);
					}
					while (property.NextVisible(false));
				}
			}

			serializedObject.ApplyModifiedProperties();

			foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(isFoldedSaveKey, callback.newValue));

			return foldout;
		}
    }
}
