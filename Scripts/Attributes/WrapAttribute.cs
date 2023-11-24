using UnityEngine;

namespace EditorAttributes
{
    public class WrapAttribute : PropertyAttribute
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
		/// Attribute to wrap over a numeric value after it surpases it's limit
		/// </summary>
		/// <param name="minValue">The min value before it wraps arround</param>
		/// <param name="maxValue">The max value before it wraps arround</param>
		public WrapAttribute(float minValue, float maxValue)
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
		/// Attribute to wrap over a numeric value after it surpases it's limit
		/// </summary>
		/// <param name="minValueX">The min value on X before it wraps arround</param>
		/// <param name="maxValueX">The max value on X before it wraps arround</param>
		/// <param name="minValueY">The min value on Y before it wraps arround</param>
		/// <param name="maxValueY">The max value on Y before it wraps arround</param>
		public WrapAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY)
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
		/// Attribute to wrap over a numeric value after it surpases it's limit
		/// </summary>
		/// <param name="minValueX">The min value on X before it wraps arround</param>
		/// <param name="maxValueX">The max value on X before it wraps arround</param>
		/// <param name="minValueY">The min value on Y before it wraps arround</param>
		/// <param name="maxValueY">The max value on Y before it wraps arround</param>
		/// <param name="minValueZ">The min value on Z before it wraps arround</param>
		/// <param name="maxValueZ">The max value on Z before it wraps arround</param>
		public WrapAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ)
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
		/// Attribute to wrap over a numeric value after it surpases it's limit
		/// </summary>
		/// <param name="minValueX">The min value on X before it wraps arround</param>
		/// <param name="maxValueX">The max value on X before it wraps arround</param>
		/// <param name="minValueY">The min value on Y before it wraps arround</param>
		/// <param name="maxValueY">The max value on Y before it wraps arround</param>
		/// <param name="minValueZ">The min value on Z before it wraps arround</param>
		/// <param name="maxValueZ">The max value on Z before it wraps arround</param>
		/// <param name="minValueW">The min value on W before it wraps arround</param>
		/// <param name="maxValueW">The max value on W before it wraps arround</param>
		public WrapAttribute(float minValueX, float maxValueX, float minValueY, float maxValueY, float minValueZ, float maxValueZ, float minValueW, float maxValueW)
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
