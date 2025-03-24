using UnityEngine;
using EditorAttributes;
using System.Collections.Generic;

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

		private Dictionary<string, float> floatValues = new()
		{
			{ "Value: 0.5", 0.5f },
			{ "Value: 1.8", 1.8f },
			{ "Value: 69.420", 69.420f } 
		};

		private List<Vector3> vectorValues = new() { Vector3.forward, Vector3.up, Vector3.right, Vector3.one, Vector3.zero };
	}
}
