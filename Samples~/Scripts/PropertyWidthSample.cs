using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/propertywidth.html")]
	public class PropertyWidthSample : MonoBehaviour
    {
		[Header("PropertyWidth Attribute:")]
		[SerializeField, PropertyWidth(300f)] private int intField;
		[SerializeField, PropertyWidth(200f)] private float floatField;
		[SerializeField, PropertyWidth(100f)] private string stringField;
	}
}
