using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/tabgroup.html")]
	public class TabGroupSample : MonoBehaviour
	{
		[Header("TabGroup Attribute:")]
		[TabGroup(nameof(intGroup), nameof(stringGroup), nameof(boolGroup))]
		[SerializeField] private Void tabGroup;

		[VerticalGroup(nameof(intField01), nameof(intField02))]
		[SerializeField, HideProperty] private Void intGroup;

		[VerticalGroup(nameof(stringField01), nameof(stringField02), nameof(stringField03))]
		[SerializeField, HideProperty] private Void stringGroup;

		[VerticalGroup(true, nameof(boolField01), nameof(boolField02))]
		[SerializeField, HideProperty] private Void boolGroup;

		[SerializeField, HideProperty] private int intField01;
		[SerializeField, HideProperty] private int intField02;

		[SerializeField, HideProperty] private string stringField01;
		[SerializeField, HideProperty] private string stringField02;
		[SerializeField, HideProperty] private string stringField03;

		[SerializeField, HideProperty] private bool boolField01;
		[SerializeField, HideProperty] private bool boolField02;
	}
}
