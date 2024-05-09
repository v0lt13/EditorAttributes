using UnityEngine;

namespace EditorAttributes
{
    public class ProgressBarAttribute : PropertyAttribute
    {
	    public float MaxValue { get; private set; }
		public float BarHeight { get; private set; }

		/// <summary>
		/// Attribute to draw a progress bar
		/// </summary>
		/// <param name="maxValue">The maximum value of the progress bar</param>
		/// <param name="barHeight">The height of the progress bar in pixels</param>
		public ProgressBarAttribute(float maxValue = 100f, float barHeight = 20f)
		{
			MaxValue = maxValue;
			BarHeight = barHeight;
		}
	}
}
