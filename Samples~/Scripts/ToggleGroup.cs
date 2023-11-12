using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
    public class ToggleGroup : MonoBehaviour
    {
		[ToggleGroup("ToggleGroup", nameof(intField), nameof(stringField), nameof(boolField))]
		[SerializeField] private Void groupHolder;

		[SerializeField, HideInInspector] private int intField;
		[SerializeField, HideInInspector] private string stringField;
		[SerializeField, HideInInspector] private bool boolField;
	}
}
