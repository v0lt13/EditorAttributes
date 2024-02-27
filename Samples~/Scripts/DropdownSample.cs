using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/dropdown.html")]
	public class DropdownSample : MonoBehaviour
	{
		[Header("Dropdown Attribute:")]
		[Dropdown(nameof(intValues))]
		[SerializeField] private int intDropdown;

		[Dropdown(nameof(stringValues))]
		[SerializeField] private string stringDropdown;

		[Dropdown(nameof(floatValues))]
		[SerializeField] private float floatDropdown;

		[SerializeField, HideInInspector] private int[] intValues = new int[] { 0, 1, 2 };

		[SerializeField, HideInInspector] private string[] stringValues = new string[] { "Value01", "Value02", "Value03" };

		[SerializeField, HideInInspector] private float[] floatValues = new float[] { 0.5f, 1.8f, 69.420f };
	}
}
