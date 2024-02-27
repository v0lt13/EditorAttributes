using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ImageAttribute))]
    public class ImageDrawer : PropertyDrawerBase
	{
        private Texture2D texture;

    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var imageAttribute = attribute as ImageAttribute;
			var imagePath = GetDynamicString(imageAttribute.ImagePath, property, imageAttribute);            

			texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);

            if (texture == null)
            {
                EditorGUILayout.HelpBox("The image asset could not be found make sure you inputted the correct filepath to a image asset", MessageType.Error);
				EditorGUILayout.PropertyField(property);
				return;
            }

            var imageWidth = imageAttribute.ImageWidth == 0f ? GetImageSize(texture).x : imageAttribute.ImageWidth;
            var imageHeight = imageAttribute.ImageHeight == 0f ? GetImageSize(texture).y : imageAttribute.ImageHeight;

			EditorGUI.DrawPreviewTexture(new Rect(position.x, position.y, imageWidth, imageHeight), texture);

            EditorGUILayout.PropertyField(property);
    	}

		protected override float GetCorrectPropertyHeight(SerializedProperty property, GUIContent label)
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
