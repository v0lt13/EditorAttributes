using UnityEngine;

namespace EditorAttributes
{
    public class PropertyWidthAttribute : PropertyAttribute
    {
	    public float LabelWidth { get; private set; }
	    public float FieldWidth { get; private set; }

		/// <summary>
		/// Attribute to adjust the width of a property's label and field
		/// </summary>
		/// <param name="labelWidth">The width of the label in pixels</param>
		/// <param name="fieldWidth">The minimum width in reserved for the fields in pixels</param>
		public PropertyWidthAttribute(float labelWidth, float fieldWidth = 50f)
        {
            LabelWidth = labelWidth;
            FieldWidth = fieldWidth;
        }
    }
}
