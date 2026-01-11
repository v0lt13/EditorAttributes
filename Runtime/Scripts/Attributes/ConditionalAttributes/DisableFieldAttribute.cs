using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to disable a field based on a condition
    /// </summary>
    public class DisableFieldAttribute : PropertyAttribute, IConditionalAttribute
    {
        public string ConditionName { get; private set; }
        public int EnumValue { get; private set; }

        /// <summary>
        /// Attribute to disable a field based on a condition
        /// </summary>
        /// <param name="conditionName">The name of the condition to evaluate</param>
        public DisableFieldAttribute(string conditionName) : base(true) => ConditionName = conditionName;

        /// <summary>
        /// Attribute to disable a field based on a condition
        /// </summary>
        /// <param name="conditionName">The name of the condition to evaluate</param>
        /// <param name="enumValue">The value of the enum condition</param>
        public DisableFieldAttribute(string conditionName, object enumValue) : this(conditionName) => EnumValue = (int)enumValue;
    }
}
