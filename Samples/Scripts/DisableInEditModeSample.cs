using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/disableineditmode.html")]
	public class DisableInEditModeSample : MonoBehaviour
	{
		[Header("DisableInEditMode Attribute:")]
		[SerializeField, DisableInEditMode] private int disabledField;
	}
}
