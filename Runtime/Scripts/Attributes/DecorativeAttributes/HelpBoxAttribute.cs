using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display a help box
	/// </summary>
	public class HelpBoxAttribute : PropertyAttribute, IDynamicStringAttribute
    {
        public string Message { get; private set; }
        public bool DrawAbove { get; private set; }

		public MessageMode MessageType { get; private set; }
		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to display a help box
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="messageType">The type of the message</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		/// <param name="drawAbove">Draws the HelpBox above the attached field</param>
		public HelpBoxAttribute(string message, MessageMode messageType = MessageMode.Log, StringInputMode stringInputMode = StringInputMode.Constant, bool drawAbove = false)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			Message = message;
			DrawAbove = drawAbove;
			MessageType = messageType;
			StringInputMode = stringInputMode;
		}
	}
}
