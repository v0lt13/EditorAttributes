using UnityEngine;

namespace EditorAttributes
{
    public class VerticalGroupAttribute : PropertyAttribute
    {
		public float WidthOffset { get; private set; }
		public bool DrawInBox { get; private set; }

        public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = false;
		}

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(bool drawInBox, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = drawInBox;
		}

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="widthOffset">By how much to offset the width of the properties in pixels</param>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(float widthOffset = 0f, bool drawInBox = false, params string[] fieldsToGroup) 
        {
			FieldsToGroup = fieldsToGroup;
            WidthOffset = widthOffset;
			DrawInBox = drawInBox;
        }
    }
}
