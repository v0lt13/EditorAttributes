using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/readonly.html")]
	public class ReadOnly : MonoBehaviour
	{
		[Header("ReadOnly Attribute:")]
		[SerializeField, ReadOnly] private int readonlyField;

		[SerializeField, ReadOnly] private int[] readonlyArray;
	}
}
