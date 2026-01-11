using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to disable a field when outside of play mode
    /// </summary>
    public class DisableInEditModeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Attribute to disable a field when outside of play mode
        /// </summary>
        public DisableInEditModeAttribute() : base(true) { }
    }
}
