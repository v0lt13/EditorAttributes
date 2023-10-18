using UnityEngine;

namespace EditorAttributes
{
	public class EnableFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;
		public readonly int enumValue;

		/// <summary>
		/// Attribute to enable fields based on a boolean condition
		/// </summary>
		/// <param name="conditionName">The name of the boolean condition to evaluate</param>
		public EnableFieldAttribute(string conditionName) => this.conditionName = conditionName;

		/// <summary>
		/// Attribute to eanble fields in the inspector based on a enum condition
		/// </summary>
		/// <param name="conditionName">The name of the enum condition to evaluate</param>
		/// <param name="enumValue">The value of the enum</param>
		public EnableFieldAttribute(string conditionName, object enumValue)
		{
			this.conditionName = conditionName;
			this.enumValue = (int)enumValue;
		}
	}
}
