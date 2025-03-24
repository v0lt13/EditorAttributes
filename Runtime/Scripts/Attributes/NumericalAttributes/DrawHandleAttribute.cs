using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Draws a handle for the appropriate type
	/// </summary>
	public class DrawHandleAttribute : PropertyAttribute, IColorAttribute
    {
		public float R { get; private set; }
		public float G { get; private set; }
		public float B { get; private set; }

		public bool UseRGB { get; private set; }
		public string HexColor { get; private set; }

		public GUIColor Color { get; private set; }
		public Space HandleSpace { get; private set; }

		/// <summary>
		/// Draws a handle for the appropriate type
		/// </summary>
		/// <param name="handleColor">The color of the handle</param>
		/// <param name="handleSpace">In which coordinate space to place the handle</param>
		public DrawHandleAttribute(GUIColor handleColor = GUIColor.Default, Space handleSpace = Space.World)
		{
			Color = handleColor;
			HandleSpace = handleSpace;
		}

		/// <summary>
		/// Draws a handle for the appropriate type
		/// </summary>
		/// <param name="r">Red amount</param>
		/// <param name="g">Green amount</param>
		/// <param name="b">Blue amount</param>
		/// <param name="handleSpace">In which coordinate space to place the handle</param>
		public DrawHandleAttribute(float r, float g, float b, Space handleSpace = Space.World)
		{
			UseRGB = true;
			R = r;
			G = g;
			B = b;
			HandleSpace = handleSpace;
		}

		/// <summary>
		/// Draws a handle for the appropriate type
		/// </summary>
		/// <param name="hexColor">The color in hexadecimal</param>
		/// <param name="handleSpace">In which coordinate space to place the handle</param>
		public DrawHandleAttribute(string hexColor, Space handleSpace = Space.World)
		{
			HexColor = hexColor;
			HandleSpace = handleSpace;
		}
	}
}
