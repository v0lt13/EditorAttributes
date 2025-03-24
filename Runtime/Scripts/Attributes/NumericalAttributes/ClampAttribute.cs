using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to clamp a numeric field between two values
	/// </summary>
	public class ClampAttribute : PropertyAttribute
    {
	    public float MinValueX { get; private set; }
	    public float MaxValueX { get; private set; }

	    public float MinValueY { get; private set; }
	    public float MaxValueY { get; private set; }

	    public float MinValueZ { get; private set; }
	    public float MaxValueZ { get; private set; }

	    public float MinValueW { get; private set; }
	    public float MaxValueW { get; private set; }

        /// <summary>
        /// Attribute to clamp a numeric field between two values
        /// </summary>
        /// <param name="minValue">The min value to clamp</param>
        /// <param name="maxValue">The max value to clamp</param>
        public ClampAttribute(float minValue, float maxValue)
        {
            MinValueX = minValue;
            MaxValueX = maxValue;

			MinValueY = minValue;
			MaxValueY = maxValue;

			MinValueZ = minValue;
			MaxValueZ = maxValue;

			MinValueW = minValue;
			MaxValueW = maxValue;
		}

		/// <summary>
		/// Attribute to clamp a numeric field between two values
		/// </summary>
		/// <param name="minValueX">The min value to clamp on X</param>
		/// <param name="maxValueX">The max value to clamp on X</param>
		/// <param name="minValueY">The min value to clamp on Y</param>
		/// <param name="maxValueY">The max value to clamp on Y</param>
		public ClampAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY)
		{
			MinValueX = minValueX;
			MaxValueX = maxValueX;

			MinValueY = minValueY;
			MaxValueY = maxValueY;

			MinValueZ = minValueX;
			MaxValueZ = maxValueX;

			MinValueW = minValueY;
			MaxValueW = maxValueY;
		}

		/// <summary>
		/// Attribute to clamp a numeric field between two values
		/// </summary>
		/// <param name="minValueX">The min value to clamp on X</param>
		/// <param name="maxValueX">The max value to clamp on X</param>
		/// <param name="minValueY">The min value to clamp on Y</param>
		/// <param name="maxValueY">The max value to clamp on Y</param>
		/// <param name="minValueZ">The min value to clamp on Z</param>
		/// <param name="maxValueZ">The max value to clamp on Z</param>
		public ClampAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ)
		{
			MinValueX = minValueX;
			MaxValueX = maxValueX;

			MinValueY = minValueY;
			MaxValueY = maxValueY;

			MinValueZ = minValueZ;
			MaxValueZ = maxValueZ;

			MinValueW = minValueX;
			MaxValueW = maxValueX;
		}

		/// <summary>
		/// Attribute to clamp a numeric field between two values
		/// </summary>
		/// <param name="minValueX">The min value to clamp on X</param>
		/// <param name="maxValueX">The max value to clamp on X</param>
		/// <param name="minValueY">The min value to clamp on Y</param>
		/// <param name="maxValueY">The max value to clamp on Y</param>
		/// <param name="minValueZ">The min value to clamp on Z</param>
		/// <param name="maxValueZ">The max value to clamp on Z</param>
		/// <param name="minValueW">The min value to clamp on W</param>
		/// <param name="maxValueW">The max value to clamp on W</param>
		public ClampAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ, float minValueW, float maxValueW)
		{
			MinValueX = minValueX;
			MaxValueX = maxValueX;

			MinValueY = minValueY;
			MaxValueY = maxValueY;

			MinValueZ = minValueZ;
			MaxValueZ = maxValueZ;

			MinValueW = minValueW;
			MaxValueW = maxValueW;
		}
	}
}
