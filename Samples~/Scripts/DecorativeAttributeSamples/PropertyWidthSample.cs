using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/propertywidth.html")]
	public class PropertyWidthSample : MonoBehaviour
    {
		[Header("PropertyWidth Attribute:")]
		[SerializeField, PropertyWidth(-100f)] private int intField;
		[SerializeField, PropertyWidth(100f)] private float floatField;
		[SerializeField, PropertyWidth(200f)] private string stringField;
	}
}
