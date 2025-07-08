using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/unitfield.html")]
	public class UnitFieldSample : MonoBehaviour
	{
		[Header("UnitField Attribute:")]
		[Rename(nameof(ConversionResultDays), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField, UnitField(Unit.Hour, Unit.Day)] private int intField;

		[Rename(nameof(ConversionResultGigabytes), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField, UnitField(Unit.Megabyte, Unit.Gigabyte)] private float floatField;

		private string ConversionResultDays() => $"{intField} Days";
		private string ConversionResultGigabytes() => $"{floatField} Gigabytes";
	}
}
