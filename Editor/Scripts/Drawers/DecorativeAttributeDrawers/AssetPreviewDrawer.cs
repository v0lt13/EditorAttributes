using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Threading.Tasks;

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

		private async void GetAssetPreview(SerializedProperty property, AssetPreviewAttribute assetPreviewAttribute, VisualElement root, Image image)
		{
			if (property.objectReferenceValue == null)
			{
				RemoveElement(root, image);
				return;
			}

			Texture2D texture = null;

			// When reassigning the object reference and the preview is not cached yet the texture will return null the first time, so we request it a second time after the first call cached it
			for (int i = 0; i < 2; i++)
			{
				if (texture != null)
					break;

				texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

				await Task.Delay(EditorAttributesSettings.instance.assetPreviewLoadTime); // Give time for the asset preview to load since is doing it asynchronously under the hood
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
