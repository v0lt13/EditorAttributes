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
		[HorizontalGroup(150f, 30f, true, nameof(intField02), nameof(stringField02), nameof(boolField02))]
		[SerializeField] private Void boxedGroupHolder;

		[SerializeField, HideProperty] private int intField01;
		[SerializeField, HideProperty] private string stringField01;
		[SerializeField, HideProperty] private bool boolField01;

		[SerializeField, HideProperty] private int intField02;
		[SerializeField, HideProperty] private string stringField02;
		[SerializeField, HideProperty] private bool boolField02;
	}
}