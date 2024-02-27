using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/prefix.html")]
	public class PrefixSample : MonoBehaviour
	{
		[Header("Prefix Attribute:")]
		[SerializeField, Prefix("num")] private int intField;
		[SerializeField, Prefix("float num", 20f)] private float floatField;
		[SerializeField, Prefix(nameof(dynamicPrefix), stringInputMode: StringInputMode.Dynamic)] private string dynamicPrefix;
	}
}
