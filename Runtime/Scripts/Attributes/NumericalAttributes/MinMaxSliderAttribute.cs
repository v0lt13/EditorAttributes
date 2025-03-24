using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to draw a min max slider
	/// </summary>
	public class MinMaxSliderAttribute : PropertyAttribute
    {
        public float MinRange { get; private set; }
        public float MaxRange { get; private set; }

        public bool ShowValues { get; private set; }

		/// <summary>
		/// Attribute to draw a min max slider
		/// </summary>
		/// <param name="minRange">The minimum range of the slider</param>
		/// <param name="maxRange">The maximum range of the slider</param>
		/// <param name="showValues">Show fields of the slider values</param>
		public MinMaxSliderAttribute(float minRange, float maxRange, bool showValues = true)
		{
			MinRange = minRange;
			MaxRange = maxRange;
			ShowValues = showValues;
		}
    }
}
