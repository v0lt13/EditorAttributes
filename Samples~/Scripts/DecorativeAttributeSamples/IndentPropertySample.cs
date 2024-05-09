using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/indentproperty.html")]
	public class IndentPropertySample : MonoBehaviour
	{
		[Header("IndentProperty Attribute:")]
		[SerializeField, IndentProperty] private int intField;
		[SerializeField, IndentProperty(30f)] private float floatField;
		[SerializeField, IndentProperty(40f)] private string stringField;
	}
}
