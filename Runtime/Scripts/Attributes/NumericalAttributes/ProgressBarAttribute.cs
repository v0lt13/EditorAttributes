using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to draw a progress bar
	/// </summary>
	public class ProgressBarAttribute : PropertyAttribute
    {
	    public float MaxValue { get; private set; }
		public float BarHeight { get; private set; }
		public bool HideLabel { get; private set; }

        /// <summary>
        /// Attribute to draw a progress bar
        /// </summary>
        /// <param name="maxValue">The maximum value of the progress bar</param>
        /// <param name="barHeight">The height of the progress bar in pixels</param>
        /// <param name="hideLabel">Hides the text inside the progress bar</param>
        public ProgressBarAttribute(float maxValue = 100f, float barHeight = 20f, bool hideLabel = false)
		{
			MaxValue = maxValue;
			BarHeight = barHeight;
		}
	}
}
