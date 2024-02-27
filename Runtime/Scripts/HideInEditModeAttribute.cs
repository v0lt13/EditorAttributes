using UnityEngine;

namespace EditorAttributes
{
    public class HideInEditModeAttribute : PropertyAttribute 
    {
		/// <summary>
		/// Attribute to hide a field when outside of play mode
		/// </summary>
		public HideInEditModeAttribute()
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{ }
    }
}
