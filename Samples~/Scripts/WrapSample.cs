using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/wrap.html")]
	public class WrapSample : MonoBehaviour
	{
		[Header("Wrap Attribute:")]
		[SerializeField, Wrap(0, 20)] private int intField;
		[SerializeField, Wrap(0f, 360f)] private float floatField;
		[Space]
		[SerializeField, Wrap(0f, 10f, -10f, 0f)] private Vector2 vector2Field;
		[SerializeField, Wrap(0f, 10f, -10f, 0f, -20f, 20f)] private Vector3 vector3Field;
	}
}
