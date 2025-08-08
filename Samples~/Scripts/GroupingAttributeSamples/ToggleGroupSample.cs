using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/togglegroup.html")]
	public class ToggleGroupSample : MonoBehaviour
	{
		[Header("ToggleGroup attribute:")]
		[ToggleGroup("ToggleGroup", nameof(intField01), nameof(stringField01), nameof(boolField01))]
		[SerializeField] private Void groupHolder;

		[ToggleGroup("BoxedToggleGroup", drawInBox: true, nameof(intField02), nameof(stringField02), nameof(boolField02))]
		[SerializeField] private bool boxedGroupHolder;

		[SerializeField, HideProperty] private int intField01;
		[SerializeField, HideProperty] private string stringField01;
		[SerializeField, HideProperty] private bool boolField01;

		[MessageBox("The toggle group has been enabled", nameof(boxedGroupHolder))]
		[SerializeField, HideProperty] private int intField02;
		[SerializeField, HideProperty] private string stringField02;
		[SerializeField, HideProperty] private bool boolField02;
	}
}
