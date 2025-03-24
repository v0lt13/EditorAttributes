using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to make a field readonly in the inspector
	/// </summary>
	public class ReadOnlyAttribute : PropertyAttribute 
    {
		/// <summary>
		/// Attribute to make a field readonly in the inspector
		/// </summary>
		public ReadOnlyAttribute()
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{ }
    }
}
