using UnityEngine;

namespace EditorAttributes
{
	public class HideFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;
		public readonly int enumValue;

		/// <summary>
		/// Attribute to hide fields based on a boolean condition
		/// </summary>
		/// <param name="conditionName">The name of the boolean condition to evaluate</param>
		public HideFieldAttribute(string conditionName) => this.conditionName = conditionName;

		/// <summary>
		/// Attribute to hide fields based on a enum condition
		/// </summary>
		/// <param name="conditionName">The name of the enum condition to evaluate</param>
		/// <param name="enumValue">The value of the enum</param>
		public HideFieldAttribute(string conditionName, object enumValue)
		{
			this.conditionName = conditionName;
			this.enumValue = (int)enumValue;
		}
	}
}
