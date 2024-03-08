using System;
using UnityEngine;

namespace EditorAttributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ButtonAttribute : PropertyAttribute, IConditionalAttribute
	{
		public string ButtonLabel { get; private set; }
		public float ButtonHeight { get; private set; }
		public bool SerializeParameters { get; private set; }

		public int EnumValue { get; private set; }
		public bool Negate { get; private set; }
		public string ConditionName { get; private set; }
		public ConditionResult ConditionResult { get; private set; }

		/// <summary>
		/// Attribute to add a button in the inspector
		/// </summary>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonHeight">The height of the button in pixels</param>
		/// <param name="serializeParameters">Have the button parameters persist between selections</param>
		public ButtonAttribute(string buttonLabel = "", float buttonHeight = 18f, bool serializeParameters = true)
		{
			ConditionName = "";
			ButtonLabel = buttonLabel;
			ButtonHeight = buttonHeight;
			SerializeParameters = serializeParameters;
		}

		/// <summary>
		/// Attribute to add a button in the inspector
		/// </summary>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		/// <param name="conditionResult">What happens to the button when the condition evaluates to true</param>
		/// <param name="negate">Negate the evaluated condition</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonHeight">The height of the button in pixels</param>
		/// <param name="serializeParameters">Have the button parameters persist between selections</param>
		public ButtonAttribute(string conditionName, ConditionResult conditionResult, bool negate = false, string buttonLabel = "", float buttonHeight = 18f, bool serializeParameters = true) 
			: this(buttonLabel, buttonHeight, serializeParameters)
		{
			ConditionResult = conditionResult;
			ConditionName = conditionName;
			Negate = negate;
		}

		/// <summary>
		/// Attribute to add a button in the inspector
		/// </summary>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		/// <param name="enumValue">The value of the enum condition</param>
		/// <param name="conditionResult">What happens to the button when the condition evaluates to true</param>
		/// <param name="negate">Negate the evaluated condition</param>
		/// <param name="buttonLabel">The label displayed on the button</param>
		/// <param name="buttonHeight">The height of the button in pixels</param>
		/// <param name="serializeParameters">Have the button parameters persist between selections</param>
		public ButtonAttribute(string conditionName, object enumValue, ConditionResult conditionResult, bool negate = false, string buttonLabel = "", float buttonHeight = 18f, bool serializeParameters = true) 
			: this(buttonLabel, buttonHeight, serializeParameters)
		{
			ConditionResult = conditionResult;
			ConditionName = conditionName;
			EnumValue = (int)enumValue;
			Negate = negate;
		}
	}
}
