using UnityEngine;

namespace EditorAttributes
{
    public class PropertyWidthAttribute : PropertyAttribute
    {
	    public float WidthOffset { get; private set; }

		/// <summary>
		/// Attribute to adjust the width of a property
		/// </summary>
		/// <param name="widthOffset">By how much to offset the width of the property in pixels</param>
		public PropertyWidthAttribute(float widthOffset) => WidthOffset = widthOffset;
    }
}
