using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/hideinplaymode.html")]
	public class HideInPlayModeSample : MonoBehaviour
	{
		[Header("HideInPlayMode Attribute:")]
		[SerializeField, HideInPlayMode] private int hiddenField;
	}
}
