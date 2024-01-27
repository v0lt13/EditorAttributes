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
		[SerializeField, HideInInspector] private Void intGroup;

		[VerticalGroup(nameof(stringField01), nameof(stringField02), nameof(stringField03))]
		[SerializeField, HideInInspector] private Void stringGroup;

		[VerticalGroup(true, nameof(boolField01), nameof(boolField02))]
		[SerializeField, HideInInspector] private Void boolGroup;

		[SerializeField, HideInInspector] private int intField01;
		[SerializeField, HideInInspector] private int intField02;

		[SerializeField, HideInInspector] private string stringField01;
		[SerializeField, HideInInspector] private string stringField02;
		[SerializeField, HideInInspector] private string stringField03;

		[SerializeField, HideInInspector] private bool boolField01;
		[SerializeField, HideInInspector] private bool boolField02;
	}
}
