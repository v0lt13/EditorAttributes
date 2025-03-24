using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to disable a field when entering play mode
	/// </summary>
	public class DisableInPlayModeAttribute : PropertyAttribute 
    {
		/// <summary>
		/// Attribute to disable a field when entering play mode
		/// </summary>
		public DisableInPlayModeAttribute()
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{ }
	}
}
