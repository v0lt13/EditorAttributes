using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/onvaluechanged.html")]
	public class OnValueChangedSample : MonoBehaviour
	{
		[Header("OnValueChanged Attribute:")]
		[SerializeField, OnValueChanged(nameof(PrintValue))] private int intField;

		private void PrintValue() => print($"Value is: {intField}");
	}
}
