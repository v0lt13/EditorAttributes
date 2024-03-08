using UnityEngine;

namespace EditorAttributes
{
    public class InlineButtonAttribute : PropertyAttribute
    {
	    public string FunctionName { get; private set; }
        public string ButtonLabel { get; private set; }
        public float ButtonWidth { get; private set; }

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
    }
}
