using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to hide a field when outside of play mode
    /// </summary>
    public class HideInEditModeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Attribute to hide a field when outside of play mode
        /// </summary>
        public HideInEditModeAttribute() : base(true) { }
    }
}
