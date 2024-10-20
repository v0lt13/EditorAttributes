using System;

namespace EditorAttributes
{
	public class HidePropertyAttribute : Attribute 
    {
		/// <summary>
		/// Attribute to hide a field in the inspector but still show it in debug view
		/// </summary>
		public HidePropertyAttribute() { }
    }
}
