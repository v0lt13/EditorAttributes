using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/button.html")]
	public class Button : MonoBehaviour
	{
		[Header("Button Attribute:")]
		[SerializeField, Space(-18f), Rename("")] private Void headerHolder;

		[Button("Button")]
		public void PrintMessage() => print("Hello World!");

		[Button]
		public void ButtonWithParams(string messageToPrint) => print(messageToPrint);

		[Button("Button", 30f)]
		private void TallButton() => print("Tall button");
	}
}
