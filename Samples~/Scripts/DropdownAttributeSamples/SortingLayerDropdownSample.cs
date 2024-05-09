using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/sortinglayerdropdown.html")]
	public class SortingLayerDropdownSample : MonoBehaviour
	{
		[Header("SortingLayerDropdown Attribute:")]
		[SerializeField, SortingLayerDropdown] private int sortingLayer;
	}
}
