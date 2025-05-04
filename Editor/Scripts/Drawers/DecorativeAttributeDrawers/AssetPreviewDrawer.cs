using UnityEngine;
using UnityEditor;
using System.Threading;
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

			var propertyField = CreatePropertyField(property);

			root.Add(propertyField);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
				var image = new Image();

				root.Add(image);

				// Register the callback later else arrays have a stroke
				ExecuteLater(root, () =>
				{
					GetAssetPreview(property, assetPreviewAttribute, root, image);

					propertyField.RegisterValueChangeCallback((changeEvent) => GetAssetPreview(property, assetPreviewAttribute, root, image));
				});
			}
            else
            {
                root.Add(new HelpBox("The attached field is not a valid asset", HelpBoxMessageType.Error));
            }

			return root;
		}

		private void GetAssetPreview(SerializedProperty property, AssetPreviewAttribute assetPreviewAttribute, VisualElement root, Image image)
		{
			if (property.objectReferenceValue == null)
			{
				RemoveElement(root, image);
				return;
			}

			int attempts = 0; // Safety measure to prevent an infinite loop if the texture cant be loaded
			Texture2D texture = null;

			while (texture == null && attempts < 3)
			{
				attempts++;
				texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

				Thread.Sleep(20); // Suspend the main thread for a bit to give time for the asset preview to load since is doing it asynchronously
			}
			
			if (texture == null)
			{
				RemoveElement(root, image);
				return;
			}

			var imageWidth = assetPreviewAttribute.PreviewWidth == 0f ? GetTextureSize(texture).x : assetPreviewAttribute.PreviewWidth;
			var imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? GetTextureSize(texture).y : assetPreviewAttribute.PreviewHeight;

			image.image = texture;
			image.style.width = imageWidth;
			image.style.height = imageHeight;

			root.Add(image);
		}
	}
}
