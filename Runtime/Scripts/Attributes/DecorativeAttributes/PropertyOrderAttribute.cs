using System;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to change the drawing order of a field
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public class PropertyOrderAttribute : Attribute
    {
        public int PropertyOrder { get; private set; }

        /// <summary>
        /// Attribute to change the drawing order of a field
        /// </summary>
        /// <param name="order">The number to other the field by</param>
    	public PropertyOrderAttribute(int order) => PropertyOrder = order;
    }
}
