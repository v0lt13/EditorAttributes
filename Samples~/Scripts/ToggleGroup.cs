using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
    public class ToggleGroup : MonoBehaviour
    {
    		[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/togglegroup.html")]
		[ToggleGroup("ToggleGroup", nameof(intField), nameof(stringField), nameof(boolField))]
		[SerializeField] private Void groupHolder;

		[SerializeField, HideInInspector] private int intField;
		[SerializeField, HideInInspector] private string stringField;
		[SerializeField, HideInInspector] private bool boolField;
	}
}
