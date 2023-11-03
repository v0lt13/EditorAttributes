using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/button.html")]
	public class Button : MonoBehaviour
	{
		[Header("Button Attribute:")]
		[Button(nameof(PrintMessage), "Button")]
		[SerializeField] private Void buttonHolder;
		[Space]
		[Button(nameof(ButtonWithParams))]
		[SerializeField] private Void buttonParamsHolder;

		public void PrintMessage() => print("Hello World!");

		public void ButtonWithParams(string messageToPrint) => print(messageToPrint);
	}
}
