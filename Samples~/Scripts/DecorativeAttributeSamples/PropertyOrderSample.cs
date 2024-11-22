using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/propertyorder.html")]
	public class PropertyOrderSample : MonoBehaviour
    {
		[Header("PropertyOrder Attribute:")]
		[SerializeField] private int intField;

		[SerializeField, PropertyOrder(3)] private int intField01;
		[SerializeField, PropertyOrder(2)] private int intField02;
		[SerializeField, PropertyOrder(1)] private int intField03;
	}
}
