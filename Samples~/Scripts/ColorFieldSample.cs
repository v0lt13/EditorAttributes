using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/colorfield.html")]
	public class ColorFieldSample : MonoBehaviour
	{
		[Header("ColorField Attribute:")]
		[SerializeField, ColorField(GUIColor.Orange)] private int intField;
		[SerializeField, ColorField(3f, 252f, 177f)] private string stringField;
		[SerializeField, ColorField("#8c508b")] private bool boolField;
	}
}
