using UnityEngine;

namespace EditorAttributes
{
	public class LineAttribute : PropertyAttribute, IColorAttribute
    {
        public float R { get; private set; }
        public float G { get; private set; }
        public float B { get; private set; }
        public float A { get; private set; }

		public float LineThickness { get; private set; }

		public string HexColor { get; private set; }
		public bool UseRGB { get; private set; }

        public GUIColor Color { get; private set; }

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="color">The color of the line</param>
		/// <param name="alpha">Alpha amount</param>
		/// <param name="lineThickness">The thickness of the line in pixels</param>
		public LineAttribute(GUIColor color = default, float alpha = 1f, float lineThickness = 3f)
		{
            Color = color;
			A = alpha;
			LineThickness = lineThickness;
		}

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="red">Red amount</param>
		/// <param name="green">Green amount</param>
		/// <param name="blue">Blue amount</param>
		/// <param name="alpha">Alpha amount</param>
		/// <param name="lineThickness">The thickness of the line in pixels</param>
		public LineAttribute(float red, float green, float blue, float alpha = 1f, float lineThickness = 3f)
        {
			UseRGB = true;
            R = red;
            G = green;
            B = blue; 
            A = alpha;
			LineThickness = lineThickness;
		}

		/// <summary>
		/// Attribute to draw a line in the inspector
		/// </summary>
		/// <param name="hexColor">The color in hexadecimal</param>
		/// <param name="alpha">Alpha amount</param>
		/// <param name="lineThickness">The thickness of the line in pixels</param>
		public LineAttribute(string hexColor, float alpha = 1f, float lineThickness = 3f)
		{
            HexColor = hexColor;
			A = alpha;
			LineThickness = lineThickness;
		}
	}
}
