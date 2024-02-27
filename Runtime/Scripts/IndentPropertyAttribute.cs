using UnityEngine;

namespace EditorAttributes
{
    public class IndentPropertyAttribute : PropertyAttribute
    {
	    public int IndentLevel { get; private set; }

        /// <summary>
        /// Attrtibute to indent a property in the inspector
        /// </summary>
        /// <param name="indentLevel">The amount to indent by</param>
        public IndentPropertyAttribute(int indentLevel = 1) => IndentLevel = indentLevel;
    }
}
