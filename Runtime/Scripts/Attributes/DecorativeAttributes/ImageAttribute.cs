using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to draw an image in the inspector
	/// </summary>
	public class ImageAttribute : PropertyAttribute, IDynamicStringAttribute
    {
        public float ImageWidth { get; private set; }
        public float ImageHeight { get; private set; }
	    public string ImagePath { get; private set; }
		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to draw an image in the inspector
		/// </summary>
		/// <param name="imagePath">The path of the image asset</param>
		/// <param name="imageWidth">The width of the image in pixels</param>
		/// <param name="imageHeight">The height of the image in pixels</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		public ImageAttribute(string imagePath, float imageWidth = 0f, float imageHeight = 0f, StringInputMode stringInputMode = StringInputMode.Constant)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			ImagePath = imagePath;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
			StringInputMode = stringInputMode;
		}
	}
}
