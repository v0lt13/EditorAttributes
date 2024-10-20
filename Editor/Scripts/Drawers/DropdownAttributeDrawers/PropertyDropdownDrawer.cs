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
				propertyField.RegisterCallback<SerializedPropertyChangeEvent>((callback) => InitializeFoldoutDrawer(root, property, fieldType, errorBox));

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
			root.schedule.Execute(() =>
			{
				var foldouts = root.Query<Foldout>().ToList();

				// The SerializedPropertyChangeEvent may be called multiple times and draw the foldout more then once so we remove the extras
				for (int i = 1; i < foldouts.Count; i++) 
					root.Remove(foldouts[i]);
			}).ExecuteLater(1);

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

				root.Add(CreatePropertyFoldout(new SerializedObject(component)));
			}
			else if (fieldType.IsSubclassOf(typeof(ScriptableObject)) || fieldType == typeof(ScriptableObject))
			{
				var scriptableObject = property.objectReferenceValue as ScriptableObject;

				root.Add(CreatePropertyFoldout(new SerializedObject(scriptableObject)));
			}
			else
			{
				root.Add(errorBox);
			}
		}

		private Foldout CreatePropertyFoldout(SerializedObject serializedObject)
        {
			var foldout = new Foldout 
			{
				text = "Properties",
				value = false
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

			return foldout;
		}
    }
}
