using UnityEngine;

namespace EditorAttributes
{
    public class FolderPathAttribute : PropertyAttribute
    {
        public bool GetRelativePath { get; private set; }

		/// <summary>
		/// Attribute to get the path of a folder
		/// </summary>
		/// <param name="getRelativePath">Get the relative path of the folder</param>
		public FolderPathAttribute(bool getRelativePath = true) => GetRelativePath = getRelativePath;
    }
}
