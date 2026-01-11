using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to hide a field when entering play mode
    /// </summary>
    public class HideInPlayModeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Attribute to hide a field when entering play mode
        /// </summary>
        public HideInPlayModeAttribute() : base(true) { }
    }
}
