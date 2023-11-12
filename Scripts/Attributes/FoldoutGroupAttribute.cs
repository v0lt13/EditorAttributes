using UnityEngine;

namespace EditorAttributes
{
    public class FoldoutGroupAttribute : PropertyAttribute
    {
		public float LabelWidth { get; private set; }
		public float FieldWidth { get; private set; }
		public string GroupName { get; private set; }

		public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display the specified fields in a foldout
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public FoldoutGroupAttribute(string groupName, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			GroupName = groupName;
			LabelWidth = 150f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display the specified fields in a foldout
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="labelWidth">The width of the field labels</param>
		/// <param name="fieldWidth">The width of the input fields</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public FoldoutGroupAttribute(string groupName, float labelWidth = 150f, float fieldWidth = 50f, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			LabelWidth = labelWidth;
			FieldWidth = fieldWidth;
			GroupName = groupName;
		}
	}
}
