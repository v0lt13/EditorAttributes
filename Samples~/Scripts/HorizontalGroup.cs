using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/horizontalgroup.html")]
	public class HorizontalGroup : MonoBehaviour
	{
		[Header("HorizontalGroup Attribute:")]
		[HorizontalGroup(nameof(intField), nameof(stringField), nameof(boolField))]
		[SerializeField] private Void group01Holder;
		[Space]
		[HorizontalGroup(80f, 60f, nameof(vectorField), nameof(arrayField))]
		[SerializeField] private Void group02Holder;

		[SerializeField, HideInInspector] private int intField;
		[SerializeField, HideInInspector] private string stringField;
		[SerializeField, HideInInspector] private bool boolField;

		[SerializeField, HideInInspector] private Vector3 vectorField;
		[SerializeField, HideInInspector] private float[] arrayField;
	}
}