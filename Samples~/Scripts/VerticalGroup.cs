using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/verticalgroup.html")]
	public class VerticalGroup : MonoBehaviour
	{
		[Header("VerticalGroup Attribute:")]

		[VerticalGroup(nameof(intField), nameof(stringField), nameof(boolField))]
		[SerializeField] private Void groupHolder;

		[SerializeField, HideInInspector] private int intField;
		[SerializeField, HideInInspector] private string stringField;
		[SerializeField, HideInInspector] private bool boolField;
	}
}
