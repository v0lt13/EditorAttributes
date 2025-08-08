using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/inlinebutton.html")]
	public class InlineButtonSample : MonoBehaviour
	{
		[Header("InlineButton Attribute:")]
		[InlineButton(nameof(PrintString))]
		[SerializeField] private string stringField;

		[InlineButton(nameof(AddValue), true, buttonLabel: "Hold to add +10", buttonWidth: 200f)]
		[SerializeField] private int intField;

		[InlineButton(nameof(DecreaseFloat), "-", 20f), InlineButton(nameof(IncreaseFloat), "+", 20f)]
		[SerializeField] private float floatField;

		private void PrintString() => print(stringField);

		private void AddValue() => intField += 10;

		private void IncreaseFloat() => floatField += 0.5f;
		private void DecreaseFloat() => floatField -= 0.5f;
	}
}
