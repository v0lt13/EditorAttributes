using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/inlinebutton.html")]
	public class InlineButtonSample : MonoBehaviour
	{
		[Header("InlineButton Attribute:")]
		[SerializeField, InlineButton(nameof(PrintString))] private string stringField;
		[SerializeField, InlineButton(nameof(AddValue), true, buttonLabel: "Hold to add +10", buttonWidth: 200f)] private int intField;

		private void PrintString() => print(stringField);

		private void AddValue() => intField += 10;
	}
}
