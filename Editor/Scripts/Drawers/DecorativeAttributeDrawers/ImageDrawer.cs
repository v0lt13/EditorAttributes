using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ImageAttribute))]
    public class ImageDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var imageAttribute = attribute as ImageAttribute;

            VisualElement root = new();
            HelpBox errorBox = new();
            Image image = new();

            root.Add(image);
            root.Add(CreatePropertyField(property));

            UpdateVisualElement(image, () =>
            {
                string imagePath = GetDynamicString(imageAttribute.ImagePath, property, imageAttribute, errorBox);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);

                if (texture == null)
                {
                    errorBox.text = "The image asset could not be found make sure you gave the correct filepath to a texture asset";
                    return;
                }

                RemoveElement(root, errorBox);

                float imageWidth = imageAttribute.ImageWidth == 0f ? GetTextureSize(texture).x : imageAttribute.ImageWidth;
                float imageHeight = imageAttribute.ImageHeight == 0f ? GetTextureSize(texture).y : imageAttribute.ImageHeight;

                image.image = texture;
                image.style.width = imageWidth;
                image.style.height = imageHeight;

                DisplayErrorBox(root, errorBox);
            });

            return root;
        }
    }
}
