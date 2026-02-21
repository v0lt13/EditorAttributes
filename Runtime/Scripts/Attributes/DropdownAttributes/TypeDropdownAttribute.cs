using System;
using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to make a dropdown of type paths
	/// </summary>
	public class TypeDropdownAttribute : PropertyAttribute
	{
		public string AssemblyName { get; private set; }
		public Type Type { get; private set; }

		/// <summary>
		/// Attribute to make a dropdown of type paths
		/// </summary>
		/// <param name="assemblyName">Filter which types are displayed by the assembly name</param>
		/// <param name="type">Filter which types are displayed by their base type<br/>(the base type itself is not part of the dropdown)</param>
		public TypeDropdownAttribute(string assemblyName = "", Type type = null)
		{
			AssemblyName = assemblyName;
			Type         = type;
		}
	}}