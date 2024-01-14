using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/required.html")]
	public class RequiredSample : MonoBehaviour
	{
		[Header("Required Attribute:")]
		[SerializeField, Required] private GameObject objectField;
	}
}
