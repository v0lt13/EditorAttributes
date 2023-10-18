using UnityEngine;

namespace EditorAttributes
{
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string message;
        public readonly bool drawProperty;
        public readonly MessageMode messageType;

		/// <summary>
		/// Attribute to display a help box
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="messageType">The type of the message</param>
		public HelpBoxAttribute(string message, MessageMode messageType = MessageMode.Log)
		{
			drawProperty = true;
			this.message = message;
			this.messageType = messageType;
		}

		/// <summary>
		/// Attribute to display a help box
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="drawProperty">Draw the property this attribute is attached to</param>
		/// <param name="messageType">The type of the message</param>
		public HelpBoxAttribute(string message, bool drawProperty, MessageMode messageType = MessageMode.Log)
		{
			this.message = message;
			this.drawProperty = drawProperty;
			this.messageType = messageType;
		}
	}
}
