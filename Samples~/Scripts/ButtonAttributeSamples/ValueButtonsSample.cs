using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/valuebuttons.html")]
	public class ValueButtonsSample : MonoBehaviour
	{
		[Header("ValueButtons Attribute:")]
		[SerializeField, ValueButtons(nameof(stringValues))] private string stringField;

		private string[] stringValues = new string[]
		{
			"Value01", "Value02", "Value03", "Value04", "Value05", "Value06"
		};
	}
}
