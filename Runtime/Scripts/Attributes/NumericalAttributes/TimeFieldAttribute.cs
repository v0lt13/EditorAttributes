using UnityEngine;

namespace EditorAttributes
{
    public enum ConvertTo
    {
        Days,
        Hours,
        Minutes,
        Seconds,
        Miliseconds
    }

    public enum TimeFormat
    {
        YearMonthWeek,
        YearMonthDay,
        WeekDayHour,
        DayHourMinute,
        HourMinuteSecond
    }

    public class TimeFieldAttribute : PropertyAttribute
    {
		public TimeFormat TimeFormat { get; private set; }
		public ConvertTo ConvertTo { get; private set; }

        /// <summary>
		/// Attribute to display a numerical field as a specified time format and convert it to a single value
		/// </summary>
        /// <param name="timeFormat">The format in which to display the time</param>
		/// <param name="convertTo">What to convert the time value into</param>
		public TimeFieldAttribute(TimeFormat timeFormat, ConvertTo convertTo)
		{
			ConvertTo = convertTo;
			TimeFormat = timeFormat;
		}
	}
}
