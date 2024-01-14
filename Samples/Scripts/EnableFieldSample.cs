using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/enablefield.html")]
	public class EnableFieldSample : MonoBehaviour
	{
		private enum States
		{
			State01 = 0,
			State02 = 1,
			State03 = 2
		}

		[Header("EnableField Attribute Boolean condition:")]
		[SerializeField] private bool enableCondition;

		[EnableField(nameof(enableCondition))]
		[SerializeField] private int enabledField;

		[Header("EnableField Attribute Enum condition:")]
		[SerializeField] private States enumEnableCondition;

		[EnableField(nameof(enumEnableCondition), States.State02)]
		[SerializeField] private int willEnableOnState02;

		[EnableField(nameof(enumEnableCondition), States.State03)]
		[SerializeField] private int willEnableOnState03;
	}
}
