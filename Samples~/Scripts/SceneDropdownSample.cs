using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/scenedropdown.html")]
	public class SceneDropdownSample : MonoBehaviour
	{
		[Header("SceneDropdown Attribute:")]
		[SerializeField, SceneDropdown] private int intField;
		[SerializeField, SceneDropdown] private string stringField;
	}
}
