using UnityEngine;

namespace EditorAttributes
{
	public enum ReferenceFixMode
	{
		None,
		Auto,
		Self,
		Children,
		Parents,
		Scene,
		Custom
	}

	/// <summary>
	/// Attribute that validates a null field in the inspector
	/// </summary>
	public class RequiredAttribute : PropertyAttribute
	{
		public ReferenceFixMode FixMode { get; private set; }

		public bool ThrowValidationError { get; private set; }
		public bool BuildKiller { get; private set; }

		public string CustomFixFunctionName { get; private set; }

		/// <summary>
		/// Attribute that validates a null field in the inspector
		/// </summary>
		/// <param name="throwValidationError">Throws an error in the console if validation fails</param>
		/// <param name="buildKiller">Throws an error during build time and cancels it if validation fails (unless build validation is disabled in the project settings)</param>
		/// <param name="fixMode">Specifies how the field should be auto-referenced by the Fix button</param>
		public RequiredAttribute(bool throwValidationError = false, bool buildKiller = false, ReferenceFixMode fixMode = ReferenceFixMode.None)
		{
			FixMode = fixMode;
			BuildKiller = buildKiller;
			ThrowValidationError = throwValidationError;
		}

		/// <summary>
		/// Attribute that validates a null field in the inspector
		/// </summary>
		/// <param name="customFixFunctionName">The name of the custom function to run by the Fix button</param>
		/// <param name="throwValidationError">Throws an error in the console if validation fails</param>
		/// <param name="buildKiller">Throws an error during build time and cancels it if validation fails (unless build validation is disabled in the project settings)</param>
		public RequiredAttribute(string customFixFunctionName, bool throwValidationError = false, bool buildKiller = false)
		{
			FixMode = ReferenceFixMode.Custom;
			BuildKiller = buildKiller;
			ThrowValidationError = throwValidationError;
			CustomFixFunctionName = customFixFunctionName;
		}
	}
}
