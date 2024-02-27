using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/indentproperty.html")]
	public class IndentPropertySample : MonoBehaviour
	{
		[Header("IndentProperty Attribute:")]
		[SerializeField, IndentProperty] private int intField;
		[SerializeField, IndentProperty(2)] private float floatField;
		[SerializeField, IndentProperty(3)] private string stringField;
	}
}
