using System;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to mark an Editor as requiring constant repainting in the inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RequiresConstantRepaintAttribute : Attribute
    {
        /// <summary>
        /// Attribute to mark an Editor as requiring constant repainting in the inspector
        /// </summary>
        public RequiresConstantRepaintAttribute() { }
    }
}
