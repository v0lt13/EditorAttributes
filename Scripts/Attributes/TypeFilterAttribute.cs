using UnityEngine;

namespace EditorAttributes
{
    public class TypeFilterAttribute : PropertyAttribute
    {
        public object[] TypesToFilter { get; private set; }

		/// <summary>
		/// Attribute to only allow asignment of objects that are or derive from the specified types
		/// </summary>
		/// <param name="typesToFilter">The type of objects to filter</param>
		public TypeFilterAttribute(params object[] typesToFilter) => TypesToFilter = typesToFilter;
    }
}
