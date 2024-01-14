using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/disablefield.html")]
	public class DisableFieldSample : MonoBehaviour
	{
		private enum States
		{
			State01 = 0,
			State02 = 1,
			State03 = 2
		}

		[Header("DisableField Attribute Boolean condition:")]
		[SerializeField] private bool disableCondition;

		[DisableField(nameof(disableCondition))]
		[SerializeField] private int disabledField;

		[Header("DisableField Attribute Enum condition:")]
		[SerializeField] private States enumDisableCondition;

		[DisableField(nameof(enumDisableCondition), States.State02)]
		[SerializeField] private int willDisableOnState02;

		[DisableField(nameof(enumDisableCondition), States.State03)]
		[SerializeField] private int willDisableOnState03;
	}
}
