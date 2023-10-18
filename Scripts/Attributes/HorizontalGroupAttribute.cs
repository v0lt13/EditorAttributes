using UnityEngine;

namespace EditorAttributes
{
    public class HorizontalGroupAttribute : PropertyAttribute
    {
        public readonly string[] fieldsToGroup;
        public readonly float labelWidth;
        public readonly float fieldWidth;

		/// <summary>
		/// Attribute to display the specified fields horizontally
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(params string[] fieldsToGroup)
		{
			this.fieldsToGroup = fieldsToGroup;
			labelWidth = 70f;
			fieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display the specified fields horizontally
		/// </summary>
		/// <param name="labelWidth">The width of the field labels</param>
		/// <param name="fieldWidth">The width of the input fields</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(float labelWidth = 70f, float fieldWidth = 50f, params string[] fieldsToGroup) 
        {
            this.fieldsToGroup = fieldsToGroup;
            this.labelWidth = labelWidth;
            this.fieldWidth = fieldWidth;
        }
    }
}
