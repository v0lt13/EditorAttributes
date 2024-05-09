using UnityEngine;

namespace EditorAttributes
{
	public class HelpBoxAttribute : PropertyAttribute
    {
        public string Message { get; private set; }
		public MessageMode MessageType { get; private set; }

		/// <summary>
		/// Attribute to display a help box
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="messageType">The type of the message</param>
		public HelpBoxAttribute(string message, MessageMode messageType = MessageMode.Log)
		{
			Message = message;
			MessageType = messageType;
		}
	}
}
