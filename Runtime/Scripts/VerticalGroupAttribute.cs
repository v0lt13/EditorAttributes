using UnityEngine;

namespace EditorAttributes
{
    public class VerticalGroupAttribute : PropertyAttribute
    {
		public float LabelWidth { get; private set; }
		public float FieldWidth { get; private set; }
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
			LabelWidth = 150f;
			FieldWidth = 50f;
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
			LabelWidth = 150f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="labelWidth">The width of the field labels in pixels</param>
		/// <param name="fieldWidth">The width of the input fields in pixels</param>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(float labelWidth = 150f, float fieldWidth = 50f, bool drawInBox = false, params string[] fieldsToGroup) 
        {
			FieldsToGroup = fieldsToGroup;
            LabelWidth = labelWidth;
            FieldWidth = fieldWidth;
			DrawInBox = drawInBox;
        }
    }
}
