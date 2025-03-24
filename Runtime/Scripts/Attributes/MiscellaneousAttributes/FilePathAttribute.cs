using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to get the path of a file
	/// </summary>
	public class FilePathAttribute : PropertyAttribute
    {
	    public bool GetRelativePath { get; private set; }
		public string Filters { get; private set; }

		/// <summary>
		/// Attribute to get the path of a file
		/// </summary>
		/// <param name="getRelativePath">Get the relative path of the file</param>
		/// <param name="filters">Filter the files by the specified extensions</param>
		public FilePathAttribute(bool getRelativePath = true, string filters = "*")
		{
			GetRelativePath = getRelativePath;
			Filters = filters;
		}
    }
}
