using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/title.html")]
	public class TitleSample : MonoBehaviour
	{
		[Header("Title Attribute:")]
		[Title("This is a title!")]
		[SerializeField] private int intField01;
		[SerializeField] private string stringField01;
		[SerializeField] private float floatField01;

		[Title("<b>Big bold title!</b>", 40, alignment: TextAnchor.MiddleRight)]
		[SerializeField] private int intField02;
		[SerializeField] private string stringField02;
		[SerializeField] private float floatField02;

		[Title("<i>This is an italic title with no line!</i>", 30, false, TextAnchor.MiddleCenter)]
		[SerializeField] private int intField03;
		[SerializeField] private string stringField03;
		[SerializeField] private float floatField03;
	}
}
