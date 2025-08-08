using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to hide a field in the inspector but still show it in debug view
	/// </summary>
	public class HidePropertyAttribute : PropertyAttribute
	{
		/// <summary>
		/// Attribute to hide a field in the inspector but still show it in debug view
		/// </summary>
		public HidePropertyAttribute()
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{ }
	}
}
