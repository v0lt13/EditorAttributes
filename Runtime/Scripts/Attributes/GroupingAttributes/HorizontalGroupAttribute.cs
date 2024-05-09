using UnityEngine;

namespace EditorAttributes
{
    public class HorizontalGroupAttribute : PropertyAttribute
    {
		public float WidthOffset { get; private set; }
		public bool DrawInBox { get; private set; }

		public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display specified fields horizontally
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = false;
		}

		/// <summary>
		/// Attribute to display specified fields horizontally
		/// </summary>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(bool drawInBox, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = drawInBox;
		}

		/// <summary>
		/// Attribute to display specified fields horizontally
		/// </summary>
		/// <param name="widthOffset">By how much to offset the width of the properties in pixels</param>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(float widthOffset = 0f, bool drawInBox = false, params string[] fieldsToGroup) 
        {
            FieldsToGroup = fieldsToGroup;
			DrawInBox = drawInBox;
			WidthOffset = widthOffset;
        }
    }
}
