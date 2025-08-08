using System;
using UnityEngine;

namespace EditorAttributes
{
	public enum TimeFormat
	{
		YearMonthWeek,
		YearMonthDay,
		WeekDayHour,
		DayHourMinute,
		HourMinuteSecond
	}

	/// <summary>
	/// Attribute to display a numerical field as a specified time format and convert it to a single value
	/// </summary>
	[Obsolete("The TimeField Attribute has been deprecated and it will be removed in the next version. Use UnitField instead.")]
	public class TimeFieldAttribute : PropertyAttribute
	{
		public TimeFormat TimeFormat { get; private set; }
		public Unit ConversionUnit { get; private set; }

		/// <summary>
		/// Attribute to display a numerical field as a specified time format and convert it to a single value
		/// </summary>
		/// <param name="timeFormat">The format in which to display the time</param>
		/// <param name="conversionUnit">The time unit to convert to</param>
		public TimeFieldAttribute(TimeFormat timeFormat, Unit conversionUnit)
		{
			ConversionUnit = conversionUnit;
			TimeFormat = timeFormat;
		}
	}
}
