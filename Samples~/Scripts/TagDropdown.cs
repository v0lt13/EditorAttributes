using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/tagdropdown.html")]
	public class TagDopdown : MonoBehaviour
	{
		[Header("TagDropdown Attribute:")]
		[SerializeField, TagDropdown] private string field;
	}
}
