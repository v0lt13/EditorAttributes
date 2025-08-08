using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
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
			var errorBox = new HelpBox("The PropertyDropdown Attribute can only be attached to objects deriving from Component or ScriptableObject", HelpBoxMessageType.Error);

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

			if (IsFieldOfType(fieldType, typeof(Component)))
			{
				var component = property.objectReferenceValue as Component;

				root.Add(CreatePropertyFoldout(new SerializedObject(component), property));
			}
			else if (IsFieldOfType(fieldType, typeof(ScriptableObject)))
			{
				var scriptableObject = property.objectReferenceValue as ScriptableObject;

				root.Add(CreatePropertyFoldout(new SerializedObject(scriptableObject), property));
			}
			else
			{
				root.Add(errorBox);
			}
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

						var field = ReflectionUtility.FindField(property.name, property.serializedObject.targetObject);

						var propertyField = CreatePropertyField(property);

						propertyField.style.unityFontStyleAndWeight = FontStyle.Normal;

						foldout.Add(propertyField);
					}
					while (property.NextVisible(false));
				}
			}

			serializedObject.ApplyModifiedProperties();

			foldout.RegisterValueChangedCallback((callback) => EditorPrefs.SetBool(foldoutSaveKey, callback.newValue));

			return foldout;
		}

		private Type GetValidPropertyType(SerializedProperty property)
		{
			var validProperty = ReflectionUtility.IsPropertyCollection(property) ? GetCollectionProperty(property) : property;
			var memberInfo = ReflectionUtility.GetValidMemberInfo(validProperty.name, validProperty);

			return ReflectionUtility.GetMemberInfoType(memberInfo);
		}

		private bool IsFieldOfType(Type fieldType, Type typeToCheck)
		{
			if (fieldType.IsArray)
			{
				return fieldType.IsSubclassOf(typeToCheck.MakeArrayType()) || fieldType == typeToCheck.MakeArrayType();
			}
			else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(List<>))
			{
				var genericType = fieldType.GetGenericArguments()[0];

				return genericType.IsSubclassOf(typeToCheck) || genericType == typeToCheck;
			}
			else
			{
				return fieldType.IsSubclassOf(typeToCheck) || fieldType == typeToCheck;
			}
		}
	}
}
