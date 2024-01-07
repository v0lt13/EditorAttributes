using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/hidelabel.html")]
	public class HideLabelSample : MonoBehaviour
	{
		[Header("HideLabel Attribute:")]
		[SerializeField, HideLabel] private string stringField;
	}
}
