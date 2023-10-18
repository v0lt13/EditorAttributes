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

    public enum ConditionResult
    {
        ShowHide,
        EnableDisable
    }

	public class ConditionalFieldAttribute : PropertyAttribute
    {
        public readonly string[] booleanNames;
        public readonly bool[] negatedValues;
        public readonly ConditionType conditionType;
        public readonly ConditionResult conditionResult;

		/// <summary>
		/// Attribute to show/hide a field based on a bunch of conditions
		/// </summary>
		/// <param name="conditionType">How to evaluate the the specified booleans</param>
		/// <param name="booleanNames">The names of the booleans to evaluate</param>
		public ConditionalFieldAttribute(ConditionType conditionType, params string[] booleanNames)
		{
			this.booleanNames = booleanNames;
			this.conditionType = conditionType;
			conditionResult = ConditionResult.ShowHide;
		}

        /// <summary>
        /// Attribute to show/hide a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="conditionResult">What happens to the property when the condition evaluates to true</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, ConditionResult conditionResult, params string[] booleanNames)
        {
            this.booleanNames = booleanNames;
            this.conditionType = conditionType;
            this.conditionResult = conditionResult;
        }

		/// <summary>
		/// Attribute to show/hide a field based on a bunch of conditions
		/// </summary>
		/// <param name="conditionType">How to evaluate the the specified booleans</param>
		/// <param name="negatedValues">Specify which booleans to negate</param>
		/// <param name="booleanNames">The names of the booleans to evaluate</param>
		public ConditionalFieldAttribute(ConditionType conditionType, bool[] negatedValues, params string[] booleanNames)
		{
			this.booleanNames = booleanNames;
			this.negatedValues = negatedValues;
			this.conditionType = conditionType;
			conditionResult = ConditionResult.ShowHide;
		}

		/// <summary>
		/// Attribute to show/hide a field based on a bunch of conditions
		/// </summary>
		/// <param name="conditionType">How to evaluate the the specified booleans</param>
		/// <param name="negatedValues">Specify which booleans to negate</param>
		/// <param name="conditionResult">What happens to the property when the condition evaluates to true</param>
		/// <param name="booleanNames">The names of the booleans to evaluate</param>
		public ConditionalFieldAttribute(ConditionType conditionType, ConditionResult conditionResult, bool[] negatedValues, params string[] booleanNames)
		{
			this.booleanNames = booleanNames;
            this.negatedValues = negatedValues;
			this.conditionType = conditionType;
            this.conditionResult = conditionResult;
		}
	}
}
