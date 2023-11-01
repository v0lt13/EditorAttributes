using UnityEngine;

namespace EditorAttributes
{
    public class HorizontalGroupAttribute : PropertyAttribute
    {
		public float LabelWidth { get; private set; }
		public float FieldWidth { get; private set; }

        public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display specified fields horizontally
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			LabelWidth = 70f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display specified fields horizontally
		/// </summary>
		/// <param name="labelWidth">The width of the field labels</param>
		/// <param name="fieldWidth">The width of the input fields</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(float labelWidth = 70f, float fieldWidth = 50f, params string[] fieldsToGroup) 
        {
            FieldsToGroup = fieldsToGroup;
            LabelWidth = labelWidth;
            FieldWidth = fieldWidth;
        }
    }
}
