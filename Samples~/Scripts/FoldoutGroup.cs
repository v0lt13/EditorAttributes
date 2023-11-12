using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
    public class FoldoutGroup : MonoBehaviour
    {
   		[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/foldoutgroup.html")]
		[FoldoutGroup("FoldoutGroup", nameof(intField), nameof(stringField), nameof(boolField))]
		[SerializeField] private Void groupHolder;

		[SerializeField, HideInInspector] private int intField;
		[SerializeField, HideInInspector] private string stringField;
		[SerializeField, HideInInspector] private bool boolField;
	}
}
