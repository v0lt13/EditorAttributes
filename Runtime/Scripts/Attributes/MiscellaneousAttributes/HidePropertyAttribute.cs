using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to hide a field in the inspector but still show it in debug view
    /// </summary>
    public class HidePropertyAttribute : PropertyAttribute
    {
        /// <summary>
        /// Attribute to hide a field in the inspector but still show it in debug view
        /// </summary>
        public HidePropertyAttribute() : base(true) { }
    }
}
