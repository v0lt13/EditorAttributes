using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/hideproperty.html")]
	public class HidePropertySample : MonoBehaviour
	{
		[Header("HideProperty Attribute:")]
		[SerializeField, HelpBox("The field is hidden in the inspector but visible in debug mode", MessageMode.None)] private Void helpBoxHolder;

		[SerializeField, HideProperty] private int hiddenField;
	}
}
