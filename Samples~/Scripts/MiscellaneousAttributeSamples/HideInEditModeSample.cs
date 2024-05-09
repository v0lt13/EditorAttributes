using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/hideineditmode.html")]
	public class HideInEditModeSample : MonoBehaviour
	{
		[Header("HideInEditMode Attribute:")]
		[SerializeField, HideInEditMode] private int hiddenField;
	}
}
