using UnityEngine;

namespace EditorAttributes
{
    public class ButtonFieldAttribute : PropertyAttribute
    {
		public string FunctionName { get; private set; }
        public string ButtonLabel { get; private set; }
        public float ButtonHeight { get; private set; }

		/// <summary>
		/// Attribute to add a button in the inspector in place of a field
		/// </summary>
		/// <param name="functionName">The name of the function to call</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonHeight">The height of the button in pixels</param>
		public ButtonFieldAttribute(string functionName, string buttonLabel = "", float buttonHeight = 18f)
		{
			FunctionName = functionName;
			ButtonLabel = buttonLabel;
			ButtonHeight = buttonHeight;
		}
    }
}
