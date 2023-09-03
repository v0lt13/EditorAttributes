using UnityEngine;

namespace EditorAttributes
{
	public class EnableFieldAttribute : PropertyAttribute
    {
        public readonly string conditionName;

        /// <summary>
        /// Attribute to enable a field based on a condition
        /// </summary>
        /// <param name="conditionName">The name of the boolean condition to evaluate</param>
        public EnableFieldAttribute(string conditionName) => this.conditionName = conditionName;
    }
}
