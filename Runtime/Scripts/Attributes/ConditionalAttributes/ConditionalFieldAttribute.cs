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

    /// <summary>
    /// Attribute to show/hide or disable/enable a field based on a bunch of conditions
    /// </summary>
    public class ConditionalFieldAttribute : PropertyAttribute
    {
        public string[] BooleanNames { get; private set; }
        public bool[] NegatedValues { get; private set; }

        public ConditionType ConditionType { get; private set; }
        public ConditionResult ConditionResult { get; private set; }

        /// <summary>
        /// Attribute to show/hide or disable/enable a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, params string[] booleanNames) : base(true)
        {
            BooleanNames = booleanNames;
            ConditionType = conditionType;
        }

        /// <summary>
        /// Attribute to show/hide or disable/enable a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="conditionResult">What happens to the property when the condition evaluates to true</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, ConditionResult conditionResult, params string[] booleanNames) : this(conditionType, booleanNames) => ConditionResult = conditionResult;

        /// <summary>
        /// Attribute to show/hide or disable/enable a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="negatedValues">Specify which booleans to negate</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, bool[] negatedValues, params string[] booleanNames) : this(conditionType, booleanNames) => NegatedValues = negatedValues;

        /// <summary>
        /// Attribute to show/hide or disable/enable a field based on a bunch of conditions
        /// </summary>
        /// <param name="conditionType">How to evaluate the the specified booleans</param>
        /// <param name="negatedValues">Specify which booleans to negate</param>
        /// <param name="conditionResult">What happens to the property when the condition evaluates to true</param>
        /// <param name="booleanNames">The names of the booleans to evaluate</param>
        public ConditionalFieldAttribute(ConditionType conditionType, ConditionResult conditionResult, bool[] negatedValues, params string[] booleanNames) : this(conditionType, conditionResult, booleanNames) => NegatedValues = negatedValues;
    }
}
