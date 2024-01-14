using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/disableinplaymode.html")]
	public class DisableInPlayModeSample : MonoBehaviour
	{
		[Header("DisableInPlayMode Attribute:")]
		[SerializeField, DisableInPlayMode] private int disabledField;
	}
}
