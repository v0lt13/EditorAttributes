using UnityEngine;

namespace EditorAttributes
{
    public class DropdownAttribute : PropertyAttribute 
	{
		public string ValueCollectionName { get; private set; }
		public string[] DisplayNames { get; private set; }

		/// <summary>
		/// Attribute to make a dropdown menu out of a collection of elements
		/// </summary>
		/// <param name="valueCollectionName">The name of the collection for the values set by the dropdown</param>
		public DropdownAttribute(string valueCollectionName) => ValueCollectionName = valueCollectionName;

		/// <summary>
		/// Attribute to make a dropdown menu out of a collection of elements
		/// </summary>
		/// <param name="valueCollectionName">The name of the collection for the values set by the dropdown</param>
		/// <param name="displayNames">Change the display name for each item inside the dropdown</param>
		public DropdownAttribute(string valueCollectionName, string[] displayNames) : this(valueCollectionName) => DisplayNames = displayNames;
	}
}
