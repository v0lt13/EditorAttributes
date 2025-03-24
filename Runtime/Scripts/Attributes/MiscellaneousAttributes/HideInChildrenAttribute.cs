using System;
using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to hide the inherited field in child classes
	/// </summary>
	public class HideInChildrenAttribute : PropertyAttribute 
    {
		public Type[] ChildTypes { get; private set; }

		/// <summary>
		/// Attribute to hide the inherited field in child classes
		/// </summary>
		/// <param name="childTypes">The field will be hidden only in these child classes</param>
		public HideInChildrenAttribute(params Type[] childTypes)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			ChildTypes = childTypes;
		}
    }
}
