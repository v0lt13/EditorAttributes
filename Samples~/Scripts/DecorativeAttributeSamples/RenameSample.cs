using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/rename.html")]
	public class RenameSample : MonoBehaviour
	{
		[Header("Rename Attribute:")]
		[SerializeField, Rename("IntField")] private int intField01;
		[Space]
		[SerializeField, Rename("pascal case mode", CaseType.Pascal)] private int intField02;
		[SerializeField, Rename("Camel Case Mode", CaseType.Camel)] private int intField03;
		[SerializeField, Rename("snake case mode", CaseType.Snake)] private int intField04;
		[SerializeField, Rename("kebab case mode", CaseType.Kebab)] private int intField05;
		[SerializeField, Rename("upper case mode", CaseType.Upper)] private int intField06;
		[SerializeField, Rename("LOWER CASE MODE", CaseType.Lower)] private int intField07;
		[Space]
		[SerializeField, Rename(nameof(dynamicName), CaseType.Unity, StringInputMode.Dynamic)] private string dynamicName;
	}
}
