using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display specified fields in a tabbed group
	/// </summary>
	public class TabGroupAttribute : PropertyAttribute
    {
		public string[] FieldsToGroup { get; private set; }

		/// <summary>
		/// Attribute to display specified fields in a tabbed group
		/// </summary>
		/// <param name="fieldsToGroup">The name of the fields to group</param>
		public TabGroupAttribute(params string[] fieldsToGroup) => FieldsToGroup = fieldsToGroup;
	}
}
