using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/line.html")]
	public class LineSample : MonoBehaviour
	{
		[Header("Line Attribute:")]
		[SerializeField] private int field01;
		[Line]
		[SerializeField] private int field02;
		[Line(GUIColor.Red)]
		[SerializeField] private int field03;
		[Line(GUIColor.Yellow, 0.1f)]
		[SerializeField] private int field04;
		[Line("#327ba8", lineThickness: 10f)]
		[SerializeField] private int field05;
	}
}
