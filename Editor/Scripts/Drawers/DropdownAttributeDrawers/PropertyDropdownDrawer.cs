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
			var fieldType = GetValidPropertyType(property);

			var root = new VisualElement();
			var propertyField = CreatePropertyField(property);
			var errorBox = new HelpBox("The PropertyDropdown Attribute can only be attached on to <b>UnityEngine.Object</b> types", HelpBoxMessageType.Error);

			ApplyBoxStyle(root);

			root.Add(propertyField);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				InitializeFoldoutDrawer(root, property, fieldType, errorBox);

				// Register the callback later to skip any initialization calls
				ExecuteLater(propertyField, () =>
				{
					propertyField.RegisterCallback<SerializedPropertyChangeEvent>((callback) =>
					{
						if (root.childCount > 1 && root.ElementAt(1) != null)
							root.RemoveAt(1);

						InitializeFoldoutDrawer(root, property, fieldType, errorBox);
					});
				});
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

			root.Add(CreatePropertyFoldout(new SerializedObject(property.objectReferenceValue), property));
		}

		private Foldout CreatePropertyFoldout(SerializedObject serializedObject, SerializedProperty serializedProperty)
		{
			var foldoutSaveKey = CreatePropertySaveKey(serializedProperty, "IsPropertyDropdownFolded");

			var foldout = new Foldout
			{
				text = "Properties",
				value = EditorPrefs.GetBool(foldoutSaveKey)
			};

			ApplyBoxStyle(foldout);

			foldout.style.paddingLeft = 15f;

			foldout.Add(new InspectorElement(serializedObject));

			serializedObject.ApplyModifiedProperties();

			foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));

			ExecuteLater(foldout, () =>
			{
				foldout.Q<Label>(className: Foldout.textUssClassName).style.unityFontStyleAndWeight = FontStyle.Bold;
				foldout.Q<ObjectField>("unity-input-m_Script")?.parent.RemoveFromHierarchy();
			});

			return foldout;
		}

		private Type GetValidPropertyType(SerializedProperty property)
		{
			var validProperty = IsPropertyCollection(property) ? GetCollectionProperty(property) : property;
			var memberInfo = ReflectionUtility.GetValidMemberInfo(validProperty.name, validProperty);

			return ReflectionUtility.GetMemberInfoType(memberInfo);
		}
	}
}
