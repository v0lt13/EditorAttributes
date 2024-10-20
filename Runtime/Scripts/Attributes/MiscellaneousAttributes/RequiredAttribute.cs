using UnityEngine;

namespace EditorAttributes
{
	public class RequiredAttribute : PropertyAttribute
    {
        public bool ThrowValidationError { get; private set; }
        public bool BuildKiller { get; private set; }

		/// <summary>
		/// Attribute that validates a null field in the inspector
		/// </summary>
		/// <param name="throwValidationError">Throws an error in the console if validation fails</param>
		public RequiredAttribute(bool throwValidationError = false) => ThrowValidationError = throwValidationError;

		/// <summary>
		/// Attribute that validates a null field in the inspector
		/// </summary>
		/// <param name="throwValidationError">Throws an error in the console if validation fails</param>
		/// <param name="buildKiller">Throws an error during build time and cancels it if validation fails</param>
		public RequiredAttribute(bool throwValidationError, bool buildKiller)
		{
			ThrowValidationError = throwValidationError;
			BuildKiller = buildKiller;
		}
	}
}
