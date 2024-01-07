using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/image.html")]
	public class ImageSample : MonoBehaviour
	{
		[Header("Image Attribute:")]
		[Image("Assets/Samples/EditorAttributes/1.9.0/AttributesSamples/EditorAttributesIcon.png")]
		[SerializeField] private int field01;

		[Image("Assets/Samples/EditorAttributes/1.9.0/AttributesSamples/EditorAttributesIcon.png", 50f, 50f)]
		[SerializeField] private int field02;
	}
}
