using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/animatorparamdropdown.html")]
	public class AnimatorParamDropdownSample : MonoBehaviour
	{
		[Header("AnimatorParamDropdown Attribute:")]
		[SerializeField] private Animator animator;
		[SerializeField, AnimatorParamDropdown(nameof(animator))] private string stringField;
	}
}
