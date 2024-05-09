using UnityEngine;

namespace EditorAttributes
{
    public class HideInChildrenAttribute : PropertyAttribute 
    {
		/// <summary>
		/// Attribute to hide the inherited field in the child classes
		/// </summary>
		public HideInChildrenAttribute()
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{ }
    }
}
