using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/typedropdown.html")]
	public class TypeDropdownSample : MonoBehaviour
	{
		[Header("TypeDropdown Attribute:")]
		[SerializeField, TypeDropdown] private string allVisibleTypes;
		[SerializeField, TypeDropdown("UnityEngine.CoreModule")] private string filteredTypes;
	}
}
