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
            if (property.propertyType != SerializedPropertyType.ObjectReference)
                return new HelpBox("The AssetPreview Attribute can only be attached on to <b>UnityEngine.Object</b> types", HelpBoxMessageType.Error);

            var assetPreviewAttribute = attribute as AssetPreviewAttribute;

            VisualElement root = new();
            Image image = new();
            PropertyField propertyField = CreatePropertyField(property);

            GetAssetPreview(property, assetPreviewAttribute, root, image);
            propertyField.RegisterValueChangeCallback((changeEvent) => GetAssetPreview(property, assetPreviewAttribute, root, image));

            root.Add(propertyField);
            root.Add(image);

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

                string assetPath = AssetDatabase.GetAssetPath(property.objectReferenceValue);

                // See if the asset is a texture first if so display the texture itself instead of it's lower res preview
                if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(Texture2D))
                {
                    texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                }
                else
                {
                    texture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                }

                await Task.Delay(EditorAttributesSettings.instance.assetPreviewLoadTime); // Give time for the asset preview to load since is doing it asynchronously under the hood
            }

            if (texture == null)
            {
                RemoveElement(root, image);
                return;
            }

            float imageWidth = assetPreviewAttribute.PreviewWidth == 0f ? GetTextureSize(texture).x : assetPreviewAttribute.PreviewWidth;
            float imageHeight = assetPreviewAttribute.PreviewHeight == 0f ? GetTextureSize(texture).y : assetPreviewAttribute.PreviewHeight;

            image.image = texture;
            image.style.width = imageWidth;
            image.style.height = imageHeight;

            root.Add(image);
        }
    }
}
