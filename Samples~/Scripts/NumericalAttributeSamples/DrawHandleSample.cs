using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/drawhandle.html")]
	public class DrawHandleSample : MonoBehaviour
	{
		[Header("DrawHandle Attribute:")]
		[SerializeField, DrawHandle] private float floatField;
		[Space]
		[SerializeField, DrawHandle(handleSpace: Space.Self)] private Vector3[] vector3Array;
		[SerializeField, DrawHandle] private SimpleTransform transformField;
		[Space]
		[SerializeField, DrawHandle(GUIColor.Green, Space.Self)] private Bounds boundsField;
	}
}
