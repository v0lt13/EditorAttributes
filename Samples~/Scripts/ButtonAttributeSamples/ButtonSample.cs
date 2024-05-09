using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/button.html")]
	public class ButtonSample : MonoBehaviour
	{
		[Header("Button Attribute:")]
		[SerializeField] private bool toggleButtons;

		[Button("Button")]
		public void PrintMessage() => print("Hello World!");

		[Button]
		public void ButtonWithParams(string messageToPrint) => print(messageToPrint);

		[Button("Button", 30f)]
		public void TallButton() => print("Tall button");

		[Button(nameof(toggleButtons), ConditionResult.EnableDisable, true)]
		public void ButtonYouCanDisable() => print("Hello World!");

		[Button(nameof(toggleButtons), ConditionResult.ShowHide, true)]
		public void ButtonYouCanHide() => print("Hello World!");
	}
}
