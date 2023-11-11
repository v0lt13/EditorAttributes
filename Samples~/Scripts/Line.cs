using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/line.html")]
	public class Line : MonoBehaviour
	{
		[Header("Line Attribute:")]
		[SerializeField] private int field01;
		[Line]
		[SerializeField] private int field02;
		[Line(LineColor.Red)]
		[SerializeField] private int field03;
		[Line(LineColor.Yellow, 0.1f)]
		[SerializeField] private int field04;
		[Line("#327ba8")]
		[SerializeField] private int field05;
	}
}
