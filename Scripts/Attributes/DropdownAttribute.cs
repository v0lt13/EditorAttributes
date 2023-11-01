using UnityEngine;

namespace EditorAttributes
{
    public class DropdownAttribute : PropertyAttribute 
	{
		public string ArrayName { get; private set; }

		/// <summary>
		/// Attribute to make a dropdown menu out of a collection of elements
		/// </summary>
		/// <param name="arrayName">The name of the array or list</param>
		public DropdownAttribute(string arrayName) => ArrayName = arrayName;
	}
}
