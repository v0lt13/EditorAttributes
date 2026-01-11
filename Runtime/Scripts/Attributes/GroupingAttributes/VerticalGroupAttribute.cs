using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to display the specified fields vertically
    /// </summary>
    public class VerticalGroupAttribute : PropertyAttribute
    {
        public string GroupName { get; private set; }
        public bool DrawInBox { get; private set; }
        public string[] FieldsToGroup { get; private set; }

        /// <summary>
        /// Attribute to display the specified fields vertically
        /// </summary>
        /// <param name="fieldsToGroup">The name of the fields to group</param>
        public VerticalGroupAttribute(params string[] fieldsToGroup)
        {
            GroupName = string.Empty;
            FieldsToGroup = fieldsToGroup;
        }

        /// <summary>
        /// Attribute to display the specified fields vertically
        /// </summary>
        /// <param name="drawInBox">Draw the group in a nice box</param>
        /// <param name="fieldsToGroup">The name of the fields to group</param>
        public VerticalGroupAttribute(bool drawInBox, params string[] fieldsToGroup) : this(fieldsToGroup) => DrawInBox = drawInBox;

        /// <summary>
        /// Attribute to display the specified fields vertically
        /// </summary>
        /// <param name="groupName">The name of the group to display as a header</param>
        /// <param name="drawInBox">Draw the group in a nice box</param>
        /// <param name="fieldsToGroup">The name of the fields to group</param>
        public VerticalGroupAttribute(string groupName, bool drawInBox, params string[] fieldsToGroup) : this(drawInBox, fieldsToGroup) => GroupName = groupName;
    }
}
