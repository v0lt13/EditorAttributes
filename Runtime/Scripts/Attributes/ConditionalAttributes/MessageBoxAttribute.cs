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

	/// <summary>
	/// Attribute to display a message box depending on a condition
	/// </summary>
	public class MessageBoxAttribute : PropertyAttribute, IConditionalAttribute, IDynamicStringAttribute
    {
		public int EnumValue { get; private set; }
		public bool DrawAbove { get; private set; }
		public string Message { get; private set; }
		public string ConditionName { get; private set; }

		public MessageMode MessageType { get; private set; }
		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to display a message box depending on a condition
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="conditionName">The condition to evaluate</param>
		/// <param name="messageType">The type of the message</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		/// <param name="drawAbove">Draws the HelpBox above the attached field</param>
		public MessageBoxAttribute(string message, string conditionName, MessageMode messageType = MessageMode.Log, StringInputMode stringInputMode = StringInputMode.Constant, bool drawAbove = false)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			Message = message;
			ConditionName = conditionName;
			MessageType = messageType;
			StringInputMode = stringInputMode;
			DrawAbove = drawAbove;
		}

		/// <summary>
		/// Attribute to display a message box depending on a condition
		/// </summary>
		/// <param name="message">The message to display</param>
		/// <param name="conditionName">The condition to evaluate</param>
		/// <param name="enumValue">The value of the enum</param>
		/// <param name="messageType">The type of the message</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		/// <param name="drawAbove">Draws the HelpBox above the attached field</param>
		public MessageBoxAttribute(string message, string conditionName, object enumValue, MessageMode messageType = MessageMode.Log, StringInputMode stringInputMode = StringInputMode.Constant, bool drawAbove = false)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			Message = message;
			EnumValue = (int)enumValue;
			ConditionName = conditionName;
			MessageType = messageType;
			DrawAbove = drawAbove;
		}
	}
}
