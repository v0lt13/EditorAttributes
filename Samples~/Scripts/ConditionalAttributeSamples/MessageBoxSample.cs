using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ConditionalAttributes/messagebox.html")]
	public class MessageBoxSample : MonoBehaviour
	{
		private enum States
		{
			State01,
			State02
		}

		[Header("MessageBox Attribute Boolean condition:")]
		[SerializeField] private bool showMessageBoxes;

		[MessageBox("This is a message box", nameof(showMessageBoxes), MessageMode.None)]
		[SerializeField] private int messageBox;

		[MessageBox("This is a <i>log</i> box", nameof(showMessageBoxes), MessageMode.Log)]
		[SerializeField] private int logBox;

		[MessageBox("This is a <b>warning</b> box", nameof(showMessageBoxes), MessageMode.Warning)]
		[SerializeField] private int warningBox;

		[MessageBox("This is an <color=#FF0000>error</color> box", nameof(showMessageBoxes), MessageMode.Error)]
		[SerializeField] private int errorBox;

		[MessageBox(nameof(dynamicMessageBox), nameof(showMessageBoxes), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField] private string dynamicMessageBox;

		[Header("MessageBox Attribute Enum condition:")]
		[SerializeField] private States states;

		[MessageBox("This is a message box", nameof(states), States.State02, MessageMode.None)]
		[SerializeField] private Void hiddenMessageBox;

		[MessageBox("This is a <i>log</i> box", nameof(states), States.State02, MessageMode.Log)]
		[SerializeField] private Void hiddenLogBox;

		[MessageBox("This is a <b>warning</b> box", nameof(states), States.State02, MessageMode.Warning)]
		[SerializeField] private Void hiddenWarningBox;

		[MessageBox("This is an <color=#FF0000>error</color> box", nameof(states), States.State02, MessageMode.Error)]
		[SerializeField] private Void hiddenErrorBox;
	}
}
