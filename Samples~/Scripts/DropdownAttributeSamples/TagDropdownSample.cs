using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/tagdropdown.html")]
	public class TagDropdownSample : MonoBehaviour
	{
		[Header("TagDropdown Attribute:")]
		[SerializeField, TagDropdown] private string field;
	}
}
