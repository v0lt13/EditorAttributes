using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/sufix.html")]
	public class Sufix : MonoBehaviour
	{
		[Header("Sufix Attribute:")]
		[SerializeField, Sufix("meters")] private float intField;
		[SerializeField, Sufix("km", 30f)] private float floatField;
	}
}
