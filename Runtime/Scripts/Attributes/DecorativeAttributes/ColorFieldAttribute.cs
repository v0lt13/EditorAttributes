using System;
using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to color a field in the inspector
	/// </summary>
	[Obsolete("This attribute has been deprecated use GUIColor instead")]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ColorFieldAttribute : PropertyAttribute, IColorAttribute
    {
		public float R { get; private set; }
		public float G { get; private set; }
		public float B { get; private set; }
		
		public bool UseRGB { get; private set; }
		public string HexColor { get; private set; }
		public string ColorFieldName { get; private set; }

		public GUIColor Color { get; private set; }

		/// <summary>
		/// Attribute to color a field in the inspector
		/// </summary>
		/// <param name="fieldColor">The color of the field</param>
		public ColorFieldAttribute(GUIColor fieldColor)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
			=> Color = fieldColor;

		/// <summary>
		/// Attribute to color a field in the inspector
		/// </summary>
		/// <param name="r">Red amount</param>
		/// <param name="g">Green amount</param>
		/// <param name="b">Blue amount</param>
		public ColorFieldAttribute(float r, float g, float b)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			UseRGB = true;
			R = r;
			G = g;
			B = b;
		}

		/// <summary>
		/// Attribute to color a field in the inspector
		/// </summary>
		/// <param name="hexColor">The color in hexadecimal</param>
		public ColorFieldAttribute(string hexColor)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
			=> HexColor = hexColor;
	}
}
