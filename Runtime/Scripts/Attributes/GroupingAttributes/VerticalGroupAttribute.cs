using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display the specified fields vertically
	/// </summary>
	public class VerticalGroupAttribute : PropertyAttribute
	{
		public bool DrawInBox { get; private set; }
		public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display the specified fields vertically
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = false;
		}

		/// <summary>
		/// Attribute to display the specified fields vertically
		/// </summary>
		/// <param name="drawInBox">Draw the group in a nice box</param>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public VerticalGroupAttribute(bool drawInBox, params string[] fieldsToGroup)
		{
			FieldsToGroup = fieldsToGroup;
			DrawInBox = drawInBox;
		}
	}
}
