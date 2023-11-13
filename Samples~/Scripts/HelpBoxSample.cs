using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/helpbox.html")]
	public class HelpBoxSample : MonoBehaviour
	{
		[Header("HelpBox Attribute:")]
		[HelpBox("This is a help box", MessageMode.None)]
		[SerializeField] private int helpBox;

		[HelpBox("This is a log box", MessageMode.Log)]
		[SerializeField] private int logBox;

		[HelpBox("This is a warning box", MessageMode.Warning)]
		[SerializeField] private int warningBox;

		[HelpBox("This is an error box", MessageMode.Error)]
		[SerializeField] private int errorBox;

		[HelpBox("This is a help box with the attached field hidden", false)]
		[SerializeField] private Void field;
	}
}
