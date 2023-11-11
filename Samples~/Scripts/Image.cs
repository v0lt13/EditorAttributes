using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/image.html")]
	public class Image : MonoBehaviour
	{
		[Header("Image Attribute:")]
		[Image("Assets/Samples/EditorAttributesIcon.png")]
		[SerializeField] private int field01;

		[Image("Assets/Samples/EditorAttributesIcon.png", 50f, 50f)]
		[SerializeField] private int field02;
	}
}
