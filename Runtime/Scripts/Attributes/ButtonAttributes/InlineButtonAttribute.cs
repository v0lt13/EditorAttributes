using UnityEngine;

namespace EditorAttributes
{
	public class InlineButtonAttribute : PropertyAttribute, IRepetableButton
    {
	    public string FunctionName { get; private set; }
        public string ButtonLabel { get; private set; }
        public float ButtonWidth { get; private set; }

        public bool IsRepetable { get; private set; }
        public long PressDelay { get; private set; }
        public long RepetitionInterval { get; private set; }

		/// <summary>
		/// Attribute to add a button next to a property
		/// </summary>
		/// <param name="functionName">The name of the function the button activates</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonWidth">The width of the button in pixels</param>
		public InlineButtonAttribute(string functionName, string buttonLabel = "", float buttonWidth = 100f)
        {
            FunctionName = functionName;
            ButtonLabel = buttonLabel;
            ButtonWidth = buttonWidth;
        }

		/// <summary>
		/// Attribute to add a button next to a property
		/// </summary>
		/// <param name="functionName">The name of the function the button activates</param>
		/// <param name="isRepetable">Makes the button repeat logic on hold</param>
		/// <param name="pressDelay">How many milliseconds to wait before the logic is executed on hold</param>
		/// <param name="repetitionInterval">The interval in milliseconds the logic will repeat</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonWidth">The width of the button in pixels</param>
		public InlineButtonAttribute(string functionName, bool isRepetable, long pressDelay = 60, long repetitionInterval = 100, string buttonLabel = "", float buttonWidth = 100f)
		{
			FunctionName = functionName;
			ButtonLabel = buttonLabel;
			ButtonWidth = buttonWidth;
			IsRepetable = isRepetable;
			PressDelay = pressDelay;
			RepetitionInterval = repetitionInterval;
		}
	}
}
