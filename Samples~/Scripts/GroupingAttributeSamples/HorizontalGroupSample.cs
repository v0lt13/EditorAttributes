using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/horizontalgroup.html")]
	public class HorizontalGroupSample : MonoBehaviour
	{
		[Header("HorizontalGroup Attribute:")]
		[HorizontalGroup(false, nameof(intField01), nameof(stringField01), nameof(boolField01))]
		[SerializeField] private Void groupHolder;
		[Space]
		[HorizontalGroup(150f, true, nameof(intField02), nameof(stringField02), nameof(boolField02))]
		[SerializeField] private Void boxedGroupHolder;

		[SerializeField, HideInInspector] private int intField01;
		[SerializeField, HideInInspector] private string stringField01;
		[SerializeField, HideInInspector] private bool boolField01;

		[SerializeField, HideInInspector] private int intField02;
		[SerializeField, HideInInspector] private string stringField02;
		[SerializeField, HideInInspector] private bool boolField02;
	}
}