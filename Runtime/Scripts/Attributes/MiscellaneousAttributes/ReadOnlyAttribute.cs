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
        public ReadOnlyAttribute() : base(true) { }
    }
}
