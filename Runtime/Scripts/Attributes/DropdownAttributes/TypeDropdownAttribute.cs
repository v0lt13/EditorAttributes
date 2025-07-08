using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to make a dropdown of type paths
	/// </summary>
	public class TypeDropdownAttribute : PropertyAttribute
	{
		public string AssemblyName { get; private set; }

		/// <summary>
		/// Attribute to make a dropdown of type paths
		/// </summary>
		/// <param name="assemblyName">Filter which types are displayed by the assembly name</param>
		public TypeDropdownAttribute(string assemblyName = "") => AssemblyName = assemblyName;
	}
}
