using UnityEngine;

namespace EditorAttributes
{
    public enum MessageMode
    {
        None,
        Log,
        Warning,
        Error
    }

    public class MessageBoxAttribute : PropertyAttribute
    {
        public readonly string message;
        public readonly string conditionName;
        public readonly bool drawProperty;
        public readonly MessageMode messageType;

        /// <summary>
        /// Attribute to display a message box depending on a condition
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="conditionName">The condition to evaluate</param>
        /// <param name="drawProperty">Draw the property this attribute is attached to</param>
        /// <param name="messageType">The type of the message</param>
        public MessageBoxAttribute(string message, string conditionName, bool drawProperty = true, MessageMode messageType = MessageMode.Log)
        {
            this.message = message;
            this.conditionName = conditionName;
            this.drawProperty = drawProperty;
            this.messageType = messageType;
        }
    }
}
