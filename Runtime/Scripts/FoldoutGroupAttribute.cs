using UnityEngine;

namespace EditorAttributes
{
    public class FoldoutGroupAttribute : PropertyAttribute
    {
		public float LabelWidth { get; private set; }
		public float FieldWidth { get; private set; }
		public string GroupName { get; private set; }
		public bool DrawInBox { get; private set; }

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
			DrawInBox = false;
			LabelWidth = 150f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display the specified fields in a foldout
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="drawInBox">Draw the fields in the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public FoldoutGroupAttribute(string groupName, bool drawInBox, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			GroupName = groupName;
			DrawInBox = drawInBox;
			LabelWidth = 150f;
			FieldWidth = 50f;
		}

		/// <summary>
		/// Attribute to display the specified fields in a foldout
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="labelWidth">The width of the field labels in pixels</param>
		/// <param name="fieldWidth">The width of the input fields in pixels</param>
		/// <param name="drawInBox">Draw the fields in the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public FoldoutGroupAttribute(string groupName, float labelWidth = 150f, float fieldWidth = 50f, bool drawInBox = false, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			LabelWidth = labelWidth;
			FieldWidth = fieldWidth;
			DrawInBox = drawInBox;
			GroupName = groupName;
		}
	}
}
