using UnityEngine;
using UnityEditor;
using System.Drawing.Imaging;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewDrawer : PropertyDrawerBase
    {
        private Texture2D texture;

    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var assetPreviewAttribute = attribute as AssetPreviewAttribute;

            DrawProperty(position, property, label);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

                if (texture == null) return;

				var imageWidth = assetPreviewAttribute.PreviewWidth == 0f ? GetImageSize(texture).x : assetPreviewAttribute.PreviewWidth;
				var imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? GetImageSize(texture).y : assetPreviewAttribute.PreviewHeight;

				EditorGUI.DrawTextureTransparent(new Rect(position.x, position.y + GetCorrectPropertyHeight(property, label), imageWidth, imageHeight), texture);
			}
            else
            {
                EditorGUILayout.HelpBox("The attached field is not a valid asset", MessageType.Error);
            }
    	}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var assetPreviewAttribute = attribute as AssetPreviewAttribute;

            if (texture == null) return GetCorrectPropertyHeight(property, label);

			var imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? GetImageSize(texture).y : assetPreviewAttribute.PreviewHeight;

			return base.GetPropertyHeight(property, label) + imageHeight;
		}

		private Vector2 GetImageSize(Texture2D texture) => new(texture.width, texture.height);
	}
}
