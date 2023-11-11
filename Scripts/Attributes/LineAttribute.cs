using UnityEngine;

namespace EditorAttributes
{
    public enum LineColor
    {
        White,
        Black,
        Gray,
        Red,
        Green,
        Lime,
        Blue,
        Cyan,
        Yellow,
        Orange,
        Brown,
        Magenta,
        Purple,
        Pink,
        UseRGB
    }

    public class LineAttribute : PropertyAttribute
    {
        public float R { get; private set; }
        public float G { get; private set; }
        public float B { get; private set; }
        public float A { get; private set; }

        public string HexColor { get; private set; }

        public LineColor LineColor { get; private set; }

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="color">The color of the line</param>
		/// <param name="alpha">Alpha amount</param>
		public LineAttribute(LineColor color = LineColor.White, float alpha = 1f)
		{
            LineColor = color;
			A = alpha;
		}

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="red">Red amount</param>
		/// <param name="green">Green amount</param>
		/// <param name="blue">Blue amount</param>
		/// <param name="alpha">Alpha amount</param>
		public LineAttribute(float red, float green, float blue, float alpha = 1f)
        {
            LineColor = LineColor.UseRGB;
            R = red;
            G = green;
            B = blue; 
            A = alpha;
        }

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="hexColor">Color in hexadecimal</param>
		/// <param name="alpha">Alpha amount</param>
		public LineAttribute(string hexColor, float alpha = 1f)
		{
            HexColor = hexColor;
			A = alpha;
		}
	}
}
