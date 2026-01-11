using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to apply a custom UI Toolkit material on a property
    /// </summary>
    public class ApplyMaterialAttribute : PropertyAttribute
    {
        public string MaterialMemberName { get; private set; }

        /// <summary>
        /// Attribute to apply a custom UI Toolkit material on a property
        /// </summary>
        /// <param name="materialMemberName">The name of the member that returns the UI Toolkit material</param>
        public ApplyMaterialAttribute(string materialMemberName) : base(true) => MaterialMemberName = materialMemberName;
    }
}
