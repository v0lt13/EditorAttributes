using System;
using UnityEngine;

namespace EditorAttributes
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Type[] TypesToFilter { get; private set; }

		/// <summary>
		/// Attribute to only allow assignment of objects that are or derive from the specified types
		/// </summary>
		/// <param name="typesToFilter">The type of objects to filter</param>
		public TypeFilterAttribute(params Type[] typesToFilter) => TypesToFilter = typesToFilter;
    }
}
