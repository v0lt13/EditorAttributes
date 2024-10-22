using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var assetPreviewAttribute = attribute as AssetPreviewAttribute;
			var root = new VisualElement();

            var propertyField = DrawProperty(property);

			root.Add(propertyField);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
				var image = new Image();

				UpdateVisualElement(root, () =>
                {
					try
					{
						if (property.objectReferenceValue == null)
						{
							if (root.Contains(image))
								root.Remove(image);

							return;
						}

						var texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

						if (texture == null)
						{
							if (root.Contains(image))
								root.Remove(image);

							return;
						}

						var imageWidth = assetPreviewAttribute.PreviewWidth == 0f ? GetTextureSize(texture).x : assetPreviewAttribute.PreviewWidth;
						var imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? GetTextureSize(texture).y : assetPreviewAttribute.PreviewHeight;

						image.image = texture;
						image.style.width = imageWidth;
						image.style.height = imageHeight;

						root.Add(image);
					}
					catch (InvalidOperationException) // Catch issues when removing from array
					{
						return;
					}
				});

				root.Add(image);
			}
            else
            {
                root.Add(new HelpBox("The attached field is not a valid asset", HelpBoxMessageType.Error));
            }

			return root;
		}
	}
}
