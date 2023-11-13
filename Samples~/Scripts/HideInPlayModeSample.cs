using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/hideinplaymode.html")]
	public class HideInPlayModeSample : MonoBehaviour
	{
		[Header("HideInPlayMode Attribute:")]
		[SerializeField, HideInPlayMode] private int hiddenField;
	}
}
