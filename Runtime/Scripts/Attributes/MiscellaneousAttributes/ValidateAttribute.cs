using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// A class to use for more advanced validation checks
	/// </summary>
	public class ValidationCheck
	{
		public string ValidationMessage { get; private set; }
		public bool PassedCheck { get; private set; }
		public bool KillBuild { get; private set; }

		public MessageMode Severety { get; private set; }

		/// <summary>
		/// Marks the validation as failed
		/// </summary>
		/// <param name="validationMessage">The message to display in the console</param>
		/// <param name="severety">The severety of the validation</param>
		/// <param name="killBuild">Throw an error during build time and cancel it</param>
		/// <returns>The validation check object</returns>
		public static ValidationCheck Fail(string validationMessage, MessageMode severety = MessageMode.Error, bool killBuild = false) => new() 
		{
			PassedCheck = false, ValidationMessage = validationMessage, Severety = severety, KillBuild = killBuild 
		};

		/// <summary>
		/// Marks the validation as passed
		/// </summary>
		/// <returns>The validation check object</returns>
		public static ValidationCheck Pass() => new() { PassedCheck = true };
	}

	/// <summary>
	/// Attribute to create custom validation
	/// </summary>
	public class ValidateAttribute : PropertyAttribute 
    {
		public string ValidationMessage { get; private set; }
		public string ConditionName { get; private set; }
		public bool BuildKiller { get; private set; }

		public MessageMode Severety { get; private set; }

		/// <summary>
		/// Attribute to create custom validation
		/// </summary>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		public ValidateAttribute(string conditionName) => ConditionName = conditionName;

		/// <summary>
		/// Attribute to create custom validation
		/// </summary>
		/// <param name="validationMessage">The message to display in the console when validation fails</param>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		/// <param name="severety">The severety of the failed validation</param>
		/// <param name="buildKiller">Throws an error during build time and cancels it if validation fails</param>
		public ValidateAttribute(string validationMessage, string conditionName, MessageMode severety = MessageMode.Error, bool buildKiller = false) : this(conditionName)
		{
			ValidationMessage = validationMessage;
			BuildKiller = buildKiller;
			Severety = severety;
		}
	}
}
