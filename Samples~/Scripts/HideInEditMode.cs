using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/hideineditmode.html")]
	public class HideInEditMode : MonoBehaviour
	{
		[Header("HideInEditMode Attribute:")]
		[SerializeField, HideInEditMode] private int hiddenField;
	}
}
