using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/image.html")]
	public class ImageSample : MonoBehaviour
	{
		private const string IMAGE_PATH = "Assets/Samples/EditorAttributes/2.0.0/AttributesSamples/EditorAttributesIcon.png";

		[Header("Image Attribute:")]
		[Image(IMAGE_PATH)]
		[SerializeField] private int field01;

		[Image(IMAGE_PATH, 50f, 50f)]
		[SerializeField] private int field02;
	}
}
