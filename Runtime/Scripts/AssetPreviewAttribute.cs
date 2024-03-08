using UnityEngine;

namespace EditorAttributes
{
    public class AssetPreviewAttribute : PropertyAttribute
    {
	    public float PreviewWidth { get; private set; }
        public float PreviewHeight { get; private set; }

		/// <summary>
		/// Attribute to preview an asset in the inspector
		/// </summary>
		/// <param name="previewWidth">The width of the preview in pixels</param>
		/// <param name="previewHeight">The height of the preview in pixels/></param>
		public AssetPreviewAttribute(float previewWidth = 0f, float previewHeight = 0f)
        {
            PreviewWidth = previewWidth;
            PreviewHeight = previewHeight;
        }
    }
}
