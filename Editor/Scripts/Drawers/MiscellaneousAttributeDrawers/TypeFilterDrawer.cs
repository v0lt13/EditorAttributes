using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TypeFilterAttribute))]
	public class TypeFilterDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var typeFilterAttribute = attribute as TypeFilterAttribute;

			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				var propertyField = CreatePropertyField(property);

				root.Add(propertyField);

				ExecuteLater(propertyField, () =>
				{
					var objectField = propertyField.Q<ObjectField>();

					objectField.objectType = property.objectReferenceValue != null ? property.objectReferenceValue.GetType() : typeFilterAttribute.TypesToFilter.FirstOrDefault();

					objectField.RegisterCallback<DragEnterEvent>(callback =>
					{
						var draggedObject = DragAndDrop.objectReferences.FirstOrDefault();

						if (draggedObject != null)
						{
							Type acceptedDraggedType = null;

							// Check if the dragged object is compatible with any of the allowed types
							var isValidType = typeFilterAttribute.TypesToFilter.Any(type =>
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
				}, 30L);
			}
			else
			{
				root.Add(new HelpBox("The attached field must derive from <b>UnityEngine.Object</b>", HelpBoxMessageType.Error));
			}

			return root;
		}
	}
}
