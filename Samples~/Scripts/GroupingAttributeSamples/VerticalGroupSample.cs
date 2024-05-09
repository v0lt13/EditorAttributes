using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/verticalgroup.html")]
	public class VerticalGroupSample : MonoBehaviour
	{
		[Header("VerticalGroup Attribute:")]
		[VerticalGroup(-300f, true, nameof(intField01), nameof(stringField01), nameof(boolField01))]
		[SerializeField] private Void groupHolder;

		[HorizontalGroup(true, nameof(boxedGroupHolder), nameof(boxedGroupHolder2))]
		[SerializeField] private Void horizontalGroupHolder;

		[VerticalGroup(true, nameof(intField02), nameof(stringField02), nameof(boolField02))]
		[SerializeField, HideInInspector] private Void boxedGroupHolder;

		[VerticalGroup(true, nameof(intField03), nameof(stringField03), nameof(boolField03))]
		[SerializeField, HideInInspector] private Void boxedGroupHolder2;

		[SerializeField, HideInInspector] private int intField01;
		[SerializeField, HideInInspector] private string stringField01;
		[SerializeField, HideInInspector] private bool boolField01;

		[SerializeField, HideInInspector] private int intField02;
		[SerializeField, HideInInspector] private string stringField02;
		[SerializeField, HideInInspector] private bool boolField02;

		[SerializeField, HideInInspector] private int intField03;
		[SerializeField, HideInInspector] private string stringField03;
		[SerializeField, HideInInspector] private bool boolField03;
	}
}
