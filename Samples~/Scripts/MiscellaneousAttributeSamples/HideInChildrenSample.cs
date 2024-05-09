using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/hideinchildren.html")]
	public class HideInChildrenSample : HideInChildrenSampleBase
	{
		[Header("HideInChildren Attribute:")]
		[HelpBox("Nothing to see here, the fields are hidden =)", MessageMode.None)] 
		[SerializeField] private Void helpBox;
	}

	public class HideInChildrenSampleBase : MonoBehaviour
	{
		[SerializeField, HideInChildren] private int intField;
		[SerializeField, HideInChildren] private float floatField;
	}
}
