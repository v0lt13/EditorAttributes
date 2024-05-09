using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/timefield.html")]
	public class TimeFieldSample : MonoBehaviour
	{
		[Header("TimeField Attribute:")]
		[Rename(nameof(ConversionResultDays), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField, TimeField(TimeFormat.YearMonthWeek, ConvertTo.Days)] private int intField;

		[Rename(nameof(ConversionResultSeconds), stringInputMode: StringInputMode.Dynamic)]
		[SerializeField, TimeField(TimeFormat.DayHourMinute, ConvertTo.Seconds)] private float floatField;

		private string ConversionResultDays() => $"{intField} Days";
		private string ConversionResultSeconds() => $"{floatField} Seconds";
	}
}
