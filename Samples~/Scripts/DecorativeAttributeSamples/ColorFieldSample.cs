using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/colorfield.html")]
	public class ColorFieldSample : MonoBehaviour
	{
		// This attribute has been deprecated, use GUIColor instead. See ColorFieldDrawer.cs for more details

		//[Header("ColorField Attribute:")]
		//[SerializeField, ColorField(GUIColor.Orange)] private int intField;
		//[SerializeField] private double doubleField;
		//[SerializeField, ColorField(3f, 252f, 177f)] private string stringField;
		//[SerializeField, ColorField("#8c508b")] private bool boolField;
		//[SerializeField] private Vector3 vectorField;
	}
}
