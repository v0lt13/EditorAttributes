using UnityEngine;

namespace EditorAttributes
{
	public class ShowFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;
        public readonly int enumValue;

        /// <summary>
        /// Attribute to show fields based on a boolean condition
        /// </summary>
        /// <param name="conditionName">The name of the boolean condition to evaluate</param>
        public ShowFieldAttribute(string conditionName) => this.conditionName = conditionName;

		/// <summary>
		/// Attribute to show fields based on a enum condition
		/// </summary>
		/// <param name="conditionName">The name of the enum condition to evaluate</param>
		/// <param name="enumValue">The value of the enum</param>
		public ShowFieldAttribute(string conditionName, object enumValue)
        {
            this.conditionName = conditionName;
            this.enumValue = (int)enumValue;
        }
    }
}
