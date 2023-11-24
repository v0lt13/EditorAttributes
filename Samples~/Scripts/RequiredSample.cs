using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/required.html")]
	public class RequiredSample : MonoBehaviour
	{
		[Header("Required Attribute:")]
		[SerializeField, Required] private GameObject objectField;
	}
}
