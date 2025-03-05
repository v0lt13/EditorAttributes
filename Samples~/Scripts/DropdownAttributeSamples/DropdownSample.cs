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

		[Dropdown(nameof(vectorValues), new string[] { "Directions/Forward", "Directions/Up", "Directions/Down", "One", "Zero" })]
		[SerializeField] private Vector3 vectorDropdown;

		private int[] intValues = new int[] { 0, 1, 2 };

		private string[] stringValues = new string[] { "Value01", "Value02", "Value03" };

		private float[] floatValues = new float[] { 0.5f, 1.8f, 69.420f };

		private Vector3[] vectorValues = new Vector3[] { Vector3.forward, Vector3.up, Vector3.right, Vector3.one, Vector3.zero };
	}
}
