using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/hidefield.html")]
	public class HideFieldSample : MonoBehaviour
	{
		private enum States
		{
			State01 = 0,
			State02 = 1,
			State03 = 2
		}

		[Header("HideField Attribute Boolean condition:")]
		[SerializeField] private bool hideCondition;

		[HideField(nameof(hideCondition))]
		[SerializeField] private int hiddenField;

		[Header("HideField Attribute Enum condition:")]
		[SerializeField] private States enumHideCondition;

		[HideField(nameof(enumHideCondition), States.State02)]
		[SerializeField] private int willHideOnState02;

		[HideField(nameof(enumHideCondition), States.State03)]
		[SerializeField] private int willHideOnState03;
	}
}
