using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/tagdropdown.html")]
	public class TagDropdownSample : MonoBehaviour
	{
		[Header("TagDropdown Attribute:")]
		[SerializeField, TagDropdown] private string field;
	}
}
