using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/suffix.html")]
	public class SuffixSample : MonoBehaviour
	{
		[Header("Suffix Attribute:")]
		[SerializeField, Suffix("meters")] private float intField;
		[SerializeField, Suffix("km", 30f)] private float floatField;
		[SerializeField, Suffix(nameof(dynamicSuffix), stringInputMode: StringInputMode.Dynamic)] private string dynamicSuffix;
	}
}
