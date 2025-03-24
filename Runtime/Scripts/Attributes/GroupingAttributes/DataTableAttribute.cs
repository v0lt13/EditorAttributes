using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to draw the members of a serialized struct or class in a table instead of a dropdown
	/// </summary>
	public class DataTableAttribute : PropertyAttribute 
    {
        public bool DrawInBox { get; private set; }
        public bool ShowLabels { get; private set; }

		/// <summary>
		/// Attribute to draw the members of a serialized struct or class in a table instead of a dropdown
		/// </summary>
		/// <param name="drawInBox">Draw the table members in a nice box</param>
		/// <param name="showLabels">Show the labels of the members</param>
		public DataTableAttribute(bool drawInBox = false, bool showLabels = true)
		{
			DrawInBox = drawInBox;
			ShowLabels = showLabels;
		}
    }
}
