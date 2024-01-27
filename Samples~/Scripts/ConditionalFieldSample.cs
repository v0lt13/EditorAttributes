using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/conditionalfield.html")]
	public class ConditionalFieldSample : MonoBehaviour
	{
		[Header("ConditionalField Attribute Hide/Show:")]
		public bool condition01;
		public bool condition02;

		[ConditionalField(ConditionType.AND, nameof(condition01), nameof(condition02))]
		[SerializeField] private int conditionalFieldAND;

		[ConditionalField(ConditionType.OR, nameof(condition01), nameof(condition02))]
		[SerializeField] private int conditionalFieldOR;

		[ConditionalField(ConditionType.NAND, nameof(condition01), nameof(condition02))]
		[SerializeField] private int conditionalFieldNAND;

		[ConditionalField(ConditionType.NOR, nameof(condition01), nameof(condition02))]
		[SerializeField] private int conditionalFieldNOR;

		[ConditionalField(ConditionType.AND, new bool[] { false, true }, nameof(condition01), nameof(condition02))]
		[SerializeField] private int conditionalFieldANDNegated;

		[Header("ConditionalField Attribute Enable/Disable:")]
		public bool _condition01;
		public bool _condition02;

		[ConditionalField(ConditionType.AND, ConditionResult.EnableDisable, nameof(_condition01), nameof(_condition02))]
		[SerializeField] private int _conditionalFieldAND;

		[ConditionalField(ConditionType.OR, ConditionResult.EnableDisable, nameof(_condition01), nameof(_condition02))]
		[SerializeField] private int _conditionalFieldOR;

		[ConditionalField(ConditionType.NAND, ConditionResult.EnableDisable, nameof(_condition01), nameof(_condition02))]
		[SerializeField] private int _conditionalFieldNAND;

		[ConditionalField(ConditionType.NOR, ConditionResult.EnableDisable, nameof(_condition01), nameof(_condition02))]
		[SerializeField] private int _conditionalFieldNOR;

		[ConditionalField(ConditionType.AND, ConditionResult.EnableDisable, new bool[] { false, true }, nameof(_condition01), nameof(_condition02))]
		[SerializeField] private int _conditionalFieldANDNegated;
	}
}
