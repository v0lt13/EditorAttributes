using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/disableinplaymode.html")]
	public class DisableInPlayMode : MonoBehaviour
	{
		[Header("DisableInPlayMode Attribute:")]
		[SerializeField, DisableInPlayMode] private int disabledField;
	}
}
