using UnityEngine;

namespace EditorAttributes
{
    public class DropdownAttribute : PropertyAttribute 
	{
		public readonly string stringArrayName;

		/// <summary>
		/// Attribute to make a dropdown menu out of a string array
		/// </summary>
		/// <param name="stringArrayName">The name of the string array</param>
		public DropdownAttribute(string stringArrayName) => this.stringArrayName = stringArrayName;
	}
}
