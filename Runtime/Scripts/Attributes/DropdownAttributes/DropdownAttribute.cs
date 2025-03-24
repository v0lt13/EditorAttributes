using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to make a dropdown menu out of a collection of elements
	/// </summary>
	public class DropdownAttribute : PropertyAttribute 
	{
		public string CollectionName { get; private set; }
		public string[] DisplayNames { get; private set; }

		/// <summary>
		/// Attribute to make a dropdown menu out of a collection of elements
		/// </summary>
		/// <param name="collectionName">The name of the collection for the values set by the dropdown</param>
		public DropdownAttribute(string collectionName) => CollectionName = collectionName;

		/// <summary>
		/// Attribute to make a dropdown menu out of a collection of elements
		/// </summary>
		/// <param name="collectionName">The name of the collection for the values set by the dropdown</param>
		/// <param name="displayNames">Change the display name for each item inside the dropdown</param>
		public DropdownAttribute(string collectionName, string[] displayNames) : this(collectionName) => DisplayNames = displayNames;
	}
}
