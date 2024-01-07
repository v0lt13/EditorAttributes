using System;
using UnityEngine;

namespace EditorAttributes
{
	public class ImageAttribute : PropertyAttribute
    {
        public float ImageWidth { get; private set; }
        public float ImageHeight { get; private set; }
	    public string ImagePath { get; private set; }

        /// <summary>
		/// Attribute to draw an image in the inspector
		/// </summary>
		/// <param name="imagePath">The path of the image asset</param>
		/// <param name="imageWidth">The width of the image</param>
		/// <param name="imageHeight">The height of the image</param>
		public ImageAttribute(string imagePath, float imageWidth = 0f, float imageHeight = 0f)
		{
			ImagePath = imagePath;
			ImageWidth = imageWidth;
			ImageHeight = imageHeight;
		}
	}
}
