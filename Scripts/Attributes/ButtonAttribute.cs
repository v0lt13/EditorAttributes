using System;
using UnityEngine;

namespace EditorAttributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ButtonAttribute : PropertyAttribute 
	{
		public string ButtonLabel { get; private set; }
		public float ButtonHeight { get; private set; }

		/// <summary>
		/// Attribute to add a button in the inspector
		/// </summary>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonHeight">The height of the button</param>
		public ButtonAttribute(string buttonLabel = "", float buttonHeight = 17f)
		{
			ButtonLabel = buttonLabel;
			ButtonHeight = buttonHeight;
		}
	}
}
