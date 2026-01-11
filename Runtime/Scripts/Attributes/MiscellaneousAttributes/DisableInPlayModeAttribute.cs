using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to disable a field when entering play mode
    /// </summary>
    public class DisableInPlayModeAttribute : PropertyAttribute
    {
        /// <summary>
        /// Attribute to disable a field when entering play mode
        /// </summary>
        public DisableInPlayModeAttribute() : base(true) { }
    }
}
