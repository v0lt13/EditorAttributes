using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/disableineditmode.html")]
	public class DisableInEditModeSample : MonoBehaviour
	{
		[Header("DisableInEditMode Attribute:")]
		[SerializeField, DisableInEditMode] private int disabledField;
	}
}
