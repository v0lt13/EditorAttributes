using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/minmaxslider.html")]
	public class MinMaxSliderSample : MonoBehaviour
	{
		[Header("MinMaxSlider Attribute:")]
		[SerializeField, MinMaxSlider(-10f, 10f)] private Vector2 vector2Field;
		[SerializeField, MinMaxSlider(0, 20f)] private Vector2Int vector2IntField;
		[Space]
		[SerializeField, MinMaxSlider(0f, 10f, false)] private Vector2 vector2FieldNoField;
		[SerializeField, MinMaxSlider(0f, 10f, false)] private Vector2Int vector2IntFieldNoField;
	}
}