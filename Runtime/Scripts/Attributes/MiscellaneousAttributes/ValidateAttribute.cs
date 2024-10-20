using UnityEngine;

namespace EditorAttributes
{
	public class ValidateAttribute : PropertyAttribute 
    {
		public string ValidationMessage { get; private set; }
		public string ConditionName { get; private set; }
		public bool BuildKiller { get; private set; }

		public MessageMode Severety { get; private set; }

		/// <summary>
		/// Attribute to create custom validation
		/// </summary>
		/// <param name="validationMessage">The message to display in the console when validation fails</param>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		/// <param name="severety">The severety of the failed validation</param>
		/// <param name="buildKiller">Throws an error during build time and cancels it if validation fails</param>
		public ValidateAttribute(string validationMessage, string conditionName, MessageMode severety = MessageMode.Error, bool buildKiller = false)
		{
			ValidationMessage = validationMessage;
			BuildKiller = buildKiller;
			ConditionName = conditionName;
			Severety = severety;
		}
	}
}
