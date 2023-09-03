using UnityEngine;

namespace EditorAttributes
{
    public enum ConditionType
    {
        AND,
        OR,
        NAND,
        NOR
    }

	public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string[] booleanNames;
        public readonly bool[] negatedValues;
        public readonly ConditionType conditionType;

        /// <summary>
        /// Attribute to show/hide a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, params string[] booleanNames)
        {
            this.booleanNames = booleanNames;
            this.conditionType = conditionType;
        }

		/// <summary>
		/// Attribute to show/hide a field based on a bunch of conditions
		/// </summary>
		/// <param name="conditionType">How to evaluate the the specified booleans</param>
		/// <param name="negatedValues">Specifiy which booleans to negate</param>
		/// <param name="booleanNames">The names of the booleans to evaluate</param>
		public ConditionalFieldAttribute(ConditionType conditionType, bool[] negatedValues, params string[] booleanNames)
		{
			this.booleanNames = booleanNames;
            this.negatedValues = negatedValues;
			this.conditionType = conditionType;
		}
	}
}
