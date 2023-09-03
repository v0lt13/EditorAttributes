using UnityEngine;

namespace EditorAttributes
{
	public class DisableFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;

		/// <summary>
		/// Attribute to disable a field based on a condition
		/// </summary>
		/// <param name="conditionName">The name of the boolean condition to evaluate</param>
		public DisableFieldAttribute(string conditionName) => this.conditionName = conditionName;
    }
}
