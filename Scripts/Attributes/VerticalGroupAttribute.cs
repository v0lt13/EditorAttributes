using UnityEngine;

namespace EditorAttributes
{
    public class VerticalGroupAttribute : PropertyAttribute
    {
		public float LabelWidth { get; private set; }
		public float FieldWidth { get; private set; }

        public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			LabelWidth = 150f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display specified fields vertically
		/// </summary>
		/// <param name="labelWidth">The width of the field labels</param>
		/// <param name="fieldWidth">The width of the input fields</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(float labelWidth = 150f, float fieldWidth = 50f, params string[] fieldsToGroup) 
        {
            FieldsToGroup = fieldsToGroup;
            LabelWidth = labelWidth;
            FieldWidth = fieldWidth;
        }
    }
}
