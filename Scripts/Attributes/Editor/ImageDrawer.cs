using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ImageAttribute))]
    public class ImageDrawer : DecoratorDrawer
    {
        private Texture2D texture;

    	public override void OnGUI(Rect position)
    	{
            var imageAttribute = attribute as ImageAttribute;

            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imageAttribute.ImagePath);

            if (texture == null)
            {
                EditorGUILayout.HelpBox("The image asset could not be found make sure you inputted the corect filepath to a image asset", MessageType.Error);
                return;
            }

            var imageWidth = imageAttribute.ImageWidth == 0f ? GetImageSize(texture).x : imageAttribute.ImageWidth;
            var imageHeight = imageAttribute.ImageHeight == 0f ? GetImageSize(texture).y : imageAttribute.ImageHeight;

			EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y, imageWidth, imageHeight), texture);
    	}

		public override float GetHeight()
		{
			var imageAttribute = attribute as ImageAttribute;

            if (texture == null) return EditorGUIUtility.standardVerticalSpacing;

            if (imageAttribute.ImageHeight == 0)
            {
                return GetImageSize(texture).y;
            }
            
			return imageAttribute.ImageHeight;
		}

        private Vector2 GetImageSize(Texture2D texture) => new(texture.width, texture.height);
	}
}
