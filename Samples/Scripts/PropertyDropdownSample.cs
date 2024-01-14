using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/propertydropdown.html")]
	public class PropertyDropdownSample : MonoBehaviour
	{
		[Header("PropertyDropdown Attribute:")]
		[SerializeField, PropertyDropdown] private BoxCollider boxCollider;
		[SerializeField, PropertyDropdown] private ExampleScriptableObject scriptableObject;
	}
}
