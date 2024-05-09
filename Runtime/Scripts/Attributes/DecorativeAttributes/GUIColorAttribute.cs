using System;
using UnityEngine;

namespace EditorAttributes
{
	public enum GUIColor
	{
		Default,
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
		Pink
	}

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class GUIColorAttribute : PropertyAttribute, IColorAttribute
    {
		public float R { get; private set; }
		public float G { get; private set; }
		public float B { get; private set; }
		public bool UseRGB { get; private set; }
		public string HexColor { get; private set; }

		public GUIColor Color { get; private set; }

		/// <summary>
		/// Attribute to color the GUI
		/// </summary>
		/// <param name="fieldColor">The color of the GUI</param>
		public GUIColorAttribute(GUIColor fieldColor) => Color = fieldColor;

		/// <summary>
		/// Attribute to color the GUI
		/// </summary>
		/// <param name="r">Red amount</param>
		/// <param name="g">Green amount</param>
		/// <param name="b">Blue amount</param>
		public GUIColorAttribute(float r, float g, float b)
		{
			UseRGB = true;
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// Attribute to color the GUI
		/// </summary>
		/// <param name="hexColor">The color in hexadecimal</param>
		public GUIColorAttribute(string hexColor) => HexColor = hexColor;
	}
}
