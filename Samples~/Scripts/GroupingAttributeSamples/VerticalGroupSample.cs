using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/verticalgroup.html")]
	public class VerticalGroupSample : MonoBehaviour
	{
		[Header("VerticalGroup Attribute:")]
		[VerticalGroup(true, nameof(intField01), nameof(stringField01), nameof(boolField01))]
		[SerializeField] private Void groupHolder;

		[HorizontalGroup(true, nameof(boxedGroupHolder), nameof(boxedGroupHolder2))]
		[SerializeField] private Void horizontalGroupHolder;

		[VerticalGroup(true, nameof(intField02), nameof(stringField02), nameof(boolField02))]
		[SerializeField, HideProperty] private Void boxedGroupHolder;

		[VerticalGroup(true, nameof(intField03), nameof(stringField03), nameof(boolField03))]
		[SerializeField, HideProperty] private Void boxedGroupHolder2;

		[SerializeField, HideProperty] private int intField01;
		[SerializeField, HideProperty] private string stringField01;
		[SerializeField, HideProperty] private bool boolField01;

		[SerializeField, HideProperty] private int intField02;
		[SerializeField, HideProperty] private string stringField02;
		[SerializeField, HideProperty] private bool boolField02;

		[SerializeField, HideProperty] private int intField03;
		[SerializeField, HideProperty] private string stringField03;
		[SerializeField, HideProperty] private bool boolField03;
	}
}
