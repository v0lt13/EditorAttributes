using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Threading.Tasks;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AssetPreviewAttribute))]
    public class AssetPreviewDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!IsSupportedPropertyType(property))
                return new HelpBox("The AssetPreview Attribute can only be attached on to <b>UnityEngine.Object</b> types", HelpBoxMessageType.Error);

            var assetPreviewAttribute = attribute as AssetPreviewAttribute;

            VisualElement root = new();
            Image image = new();
            PropertyField propertyField = CreatePropertyField(property);

            propertyField.RegisterValueChangeCallback((changeEvent) => GetAssetPreview(property, assetPreviewAttribute, root, image));

            root.Add(propertyField);
            root.Add(image);

            GetAssetPreview(property, assetPreviewAttribute, root, image);

            return root;
        }

        protected override bool IsSupportedPropertyType(SerializedProperty property) => property.propertyType == SerializedPropertyType.ObjectReference;

        private async void GetAssetPreview(SerializedProperty property, AssetPreviewAttribute assetPreviewAttribute, VisualElement root, Image image)
        {
            if (property.objectReferenceValue == null)
            {
                RemoveElement(root, image);
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);
            Texture2D texture = null;
            Vector2 previewSize;

            // See if the asset is a texture first if so display the texture itself instead of it's lower res preview
            if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(Texture2D))
            {
                var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);

                if (textureImporter.spriteImportMode == SpriteImportMode.Multiple)
                {
                    var selectedSprite = sprites.First((importedTexture) => importedTexture == property.objectReferenceValue) as Sprite;

                    texture = selectedSprite.texture;
                    image.sprite = selectedSprite;
                    previewSize = selectedSprite.rect.size;
                }
                else
                {
                    texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);

                    image.image = texture;
                    previewSize = new Vector2(texture.width, texture.height);
                }
            }
            else
            {
                // When reassigning the object reference and the preview is not cached yet the texture will return null the first time, so we request it a second time after the first call cached it
                for (int i = 0; i < 2; i++)
                {
                    if (texture != null)
                        break;

                    texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);

                    await Task.Delay(EditorAttributesSettings.instance.assetPreviewLoadTime); // Give time for the asset preview to load since is doing it asynchronously under the hood
                }

                image.image = texture;
                previewSize = new Vector2(texture.width, texture.height);
            }

            float imageWidth = assetPreviewAttribute.PreviewWidth == 0f ? previewSize.x : assetPreviewAttribute.PreviewWidth;
            float imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? previewSize.y : assetPreviewAttribute.PreviewHeight;

            image.style.width = imageWidth;
            image.style.height = imageHeight;

            root.Add(image);
        }
    }
}
