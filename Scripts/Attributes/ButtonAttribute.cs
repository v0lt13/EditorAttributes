using UnityEngine;

namespace EditorAttributes
{
    public class ButtonAttribute : PropertyAttribute 
	{
		public readonly string functionName;
		public readonly string buttonLabel;

		/// <summary>
		/// Attribute to add a button in the inspector
		/// </summary>
		/// <param name="functionName">The name of the function to call when the button is pressed</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		public ButtonAttribute(string functionName, string buttonLabel = "")
		{
			this.functionName = functionName;
			this.buttonLabel = buttonLabel;
		}
	}
}
