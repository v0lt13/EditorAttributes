using UnityEngine;

namespace EditorAttributes
{
	public class DisableFieldAttribute : PropertyAttribute, IConditionalAttribute
    {
        public string ConditionName { get; private set; }
		public int EnumValue { get; private set; }

		/// <summary>
		/// Attribute to disable a field based on a condition
		/// </summary>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		public DisableFieldAttribute(string conditionName) => ConditionName = conditionName;

		/// <summary>
		/// Attribute to disable a field based on a condition
		/// </summary>
		/// <param name="conditionName">The name of the condition to evaluate</param>
		/// <param name="enumValue">The value of the enum condition</param>
		public DisableFieldAttribute(string conditionName, object enumValue)
		{
			ConditionName = conditionName;
			EnumValue = (int)enumValue;
		}
	}
}
