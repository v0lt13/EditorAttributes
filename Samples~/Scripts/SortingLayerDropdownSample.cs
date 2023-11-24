using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/sortinglayerdropdown.html")]
	public class SortingLayerDropdownSample : MonoBehaviour
	{
		[Header("SortingLayerDropdown Attribute:")]
		[SerializeField, SortingLayerDropdown] private int sortingLayer;
	}
}
