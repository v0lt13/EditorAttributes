using UnityEngine;

namespace EditorAttributes
{
    public class HorizontalGroupAttribute : PropertyAttribute
    {
        public readonly string[] fieldsToGroup;
        public readonly float labelWidth;
        public readonly float fieldWidth;

        /// <summary>
        /// Attribute to display the specified fields horizontaly
        /// </summary>
        /// <param name="labelWidth">The width of the field labels</param>
        /// <param name="fieldWidth">The width of the input field</param>
        /// <param name="fieldsToGroup">The name of the fields to group</param>
		public HorizontalGroupAttribute(float labelWidth = 50f, float fieldWidth = 50f, params string[] fieldsToGroup) 
        {
            this.fieldsToGroup = fieldsToGroup;
            this.labelWidth = labelWidth;
            this.fieldWidth = fieldWidth;
        }
    }
}
