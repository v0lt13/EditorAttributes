using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to call a function when the value of the attached property changes
	/// </summary>
	public class OnValueChangedAttribute : PropertyAttribute
    {
        public string FunctionName { get; private set; }

        /// <summary>
        /// Attribute to call a function when the value of the attached property changes
        /// </summary>
        /// <param name="functionName">The name of the function to call</param>
    	public OnValueChangedAttribute(string functionName) => FunctionName = functionName;
    }
}
