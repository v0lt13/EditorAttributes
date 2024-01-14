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

		[MessageBox("This is a log box", nameof(showMessageBoxes), MessageMode.Log)]
		[SerializeField] private int logBox;

		[MessageBox("This is a warning box", nameof(showMessageBoxes), MessageMode.Warning)]
		[SerializeField] private int warningBox;

		[MessageBox("This is an error box", nameof(showMessageBoxes), MessageMode.Error)]
		[SerializeField] private int errorBox;

		[Header("MessageBox Attribute Enum condition:")]
		[SerializeField] private States states;

		[MessageBox("This is a message box with the attached field hidden", nameof(states), States.State02, false, MessageMode.None)]
		[SerializeField] private Void hiddenMessageBox;

		[MessageBox("This is a log box with the attached hidden", nameof(states), States.State02, false, MessageMode.Log)]
		[SerializeField] private Void hiddenLogBox;

		[MessageBox("This is a warning box with the attached hidden", nameof(states), States.State02, false, MessageMode.Warning)]
		[SerializeField] private Void hiddenWarningBox;

		[MessageBox("This is an error box with the attached hidden", nameof(states), States.State02, false, MessageMode.Error)]
		[SerializeField] private Void hiddenErrorBox;
	}
}
