using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/clamp.html")]
	public class ClampSample : MonoBehaviour
    {
		[Header("Clamp Attribute:")]
		[SerializeField, Clamp(-10, 10)] private int intField;
		[SerializeField, Clamp(-0.5f, 10.5f)] private float floatField;
		[Space]
		[SerializeField, Clamp(-10f, 10f, -5f, 5f)] private Vector2 vector2Field;
		[SerializeField, Clamp(-10f, 10f, -5f, 5f, -1f, 1f)] private Vector3 vector3Field;
	}
}
