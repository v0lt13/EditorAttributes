using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/inlinebutton.html")]
	public class InlineButtonSample : MonoBehaviour
	{
		[Header("InlineButton Attribute:")]
		[SerializeField, InlineButton(nameof(InlineButton))] private int intField;
		[SerializeField, InlineButton(nameof(PrintString), "Press Me!", 200f)] private string stringField;

		private void InlineButton() => print("Hello World!");

		private void PrintString() => print(stringField);
	}
}
