using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/image.html")]
	public class ImageSample : MonoBehaviour
	{
		[Header("Image Attribute:")]
		[SerializeField, FilePath(filters: "png")] private string imagePath;

		[Image(nameof(imagePath), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField] private int field01;

		[Image(nameof(imagePath), 50f, 50f, StringInputMode.Dynamic)]
		[SerializeField] private int field02;
	}
}
