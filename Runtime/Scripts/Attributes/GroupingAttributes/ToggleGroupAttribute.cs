using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display the specified fields in a toggleble group
	/// </summary>
	public class ToggleGroupAttribute : PropertyAttribute
	{
		public string GroupName { get; private set; }
		public bool DrawInBox { get; private set; }

		public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display the specified fields in a toggleble group
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public ToggleGroupAttribute(string groupName, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			GroupName = groupName;
			DrawInBox = false;
		}

		/// <summary>
		/// Attribute to display the specified fields in a toggleble group
		/// </summary>
		/// <param name="groupName">The name of the group</param>
		/// <param name="drawInBox">Draw the fields in the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public ToggleGroupAttribute(string groupName, bool drawInBox, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			GroupName = groupName;
			DrawInBox = drawInBox;
		}
	}
}
