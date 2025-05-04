using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var requiredAttribute = attribute as RequiredAttribute;

			var propertyField = CreatePropertyField(property);

			root.Add(propertyField);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				var helpBox = new HelpBox($"The field <b>{property.displayName}</b> must be assigned", HelpBoxMessageType.Error);

				if (CanApplyGlobalColor)
				{
					helpBox.style.color = EditorExtension.GLOBAL_COLOR;
					helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
				}

				propertyField.RegisterCallback<SerializedPropertyChangeEvent>((e) =>
				{
					if (property.objectReferenceValue == null)
					{
						root.Add(helpBox);
					}
					else
					{
						RemoveElement(root, helpBox);
					}
				});
			}
			else
			{
				root.Add(new HelpBox("The attached field must derive from <b>UnityEngine.Object</b>", HelpBoxMessageType.Error));
			}

			return root;
		}
	}
}
