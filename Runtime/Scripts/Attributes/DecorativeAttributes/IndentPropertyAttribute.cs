using UnityEngine;

namespace EditorAttributes
{
    public class IndentPropertyAttribute : PropertyAttribute
    {
	    public float IndentLevel { get; private set; }

        /// <summary>
        /// Attrtibute to indent a property in the inspector
        /// </summary>
        /// <param name="indentLevel">The amount to indent by in pixels</param>
        public IndentPropertyAttribute(float indentLevel = 20f) => IndentLevel = indentLevel;
    }
}
