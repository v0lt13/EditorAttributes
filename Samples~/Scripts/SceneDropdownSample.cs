using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/scenedropdown.html")]
	public class SceneDropdownSample : MonoBehaviour
	{
		[Header("SceneDropdown Attribute:")]
		[SerializeField, SceneDropdown] private int intField;
		[SerializeField, SceneDropdown] private string stringField;
	}
}
