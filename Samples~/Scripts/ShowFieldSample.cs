using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/showfield.html")]
	public class ShowFieldSample : MonoBehaviour
	{
		private enum States
		{
			State01 = 0,
			State02 = 1,
			State03 = 2
		}

		[Header("ShowField Attribute Boolean condition:")]
		[SerializeField] private bool showCondition;

		[ShowField(nameof(showCondition))]
		[SerializeField] private int shownField;

		[Header("ShowField Attribute Enum condition:")]
		[SerializeField] private States enumShowCondition;

		[ShowField(nameof(enumShowCondition), States.State02)]
		[SerializeField] private int willShowOnState02;

		[ShowField(nameof(enumShowCondition), States.State03)]
		[SerializeField] private int willShowOnState03;
	}
}
