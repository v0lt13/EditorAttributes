using UnityEngine;

namespace EditorAttributes
{
	public class DisableFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;
		public readonly int enumValue;

		/// <summary>
		/// Attribute to disable fields based on a boolean condition
		/// </summary>
		/// <param name="conditionName">The name of the boolean condition to evaluate</param>
		public DisableFieldAttribute(string conditionName) => this.conditionName = conditionName;

		/// <summary>
		/// Attribute to disable fields in the inspector based on a enum condition
		/// </summary>
		/// <param name="conditionName">The name of the enum condition to evaluate</param>
		/// <param name="enumValue">The value of the enum</param>
		public DisableFieldAttribute(string conditionName, object enumValue)
		{
			this.conditionName = conditionName;
			this.enumValue = (int)enumValue;
		}
	}
}
