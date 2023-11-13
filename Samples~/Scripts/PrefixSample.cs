using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/prefix.html")]
	public class PrefixSample : MonoBehaviour
	{
		[Header("Prefix Attribute:")]
		[SerializeField, Prefix("num")] private int intField;
		[SerializeField, Prefix("float num", 20f)] private float floatField;
	}
}
