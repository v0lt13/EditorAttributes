using UnityEngine;

namespace EditorAttributes
{
    public class ProgressBarAttribute : PropertyAttribute, IColorAttribute
    {
	    public float MaxValue { get; private set; }
		public float BarHeight { get; private set; }

		public float R { get; private set; }
		public float G { get; private set; }
		public float B { get; private set; }
		public bool UseRGB { get; private set; }
		public string HexColor { get; private set; }

        public GUIColor Color { get; private set; }

		/// <summary>
		/// Attribute to draw a progress bar
		/// </summary>
		/// <param name="color">The color of the bar filling</param>
		/// <param name="maxValue">The maximum value of the progress bar</param>
		/// <param name="barHeight">The height of the progress bar</param>
		public ProgressBarAttribute(GUIColor color = default, float maxValue = 100f, float barHeight = 20f)
		{
			MaxValue = maxValue;
			BarHeight = barHeight;
			Color = color;
		}

		/// <summary>
		/// Attribute to draw a progress bar
		/// </summary>
		/// <param name="red">Filling bar red amount</param>
		/// <param name="green">Filling bar green amount</param>
		/// <param name="blue">Filling bar blue amount</param>
		/// <param name="maxValue">The maximum value of the progress bar</param>
		/// <param name="barHeight">The height of the progress bar</param>
		public ProgressBarAttribute(float red, float green, float blue, float maxValue = 100f, float barHeight = 20f)
		{
			R = red;
			G = green;
			B = blue;
			UseRGB = true;
			BarHeight = barHeight;
			MaxValue = maxValue;
		}

		/// <summary>
		/// Attribute to draw a progress bar
		/// </summary>
		/// <param name="hexColor">The color of the bar filling in hexadecimals</param>
		/// <param name="maxValue">The maximum value of the progress bar</param>
		/// <param name="barHeight">The height of the progress bar</param>
		public ProgressBarAttribute(string hexColor, float maxValue = 100f, float barHeight = 20f)
		{
			HexColor = hexColor;
			BarHeight = barHeight;
			MaxValue = maxValue;
		}
	}
}
