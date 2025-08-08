using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TimeFieldAttribute)), Obsolete]
	public class TimeFieldDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var timeFieldAttribute = attribute as TimeFieldAttribute;

			var timeVectorSaveKeyX = CreatePropertySaveKey(property, "TimeX");
			var timeVectorSaveKeyY = CreatePropertySaveKey(property, "TimeY");
			var timeVectorSaveKeyZ = CreatePropertySaveKey(property, "TimeZ");

			var root = new VisualElement();
			var errorBox = new HelpBox();

			if (!UnitConverter.IsUnitInCategory(timeFieldAttribute.ConversionUnit, UnitCategory.Time))
			{
				errorBox.text = $"The conversion unit <b>{timeFieldAttribute.ConversionUnit}</b> is not a valid time unit";

				DisplayErrorBox(root, errorBox);
				return root;
			}

			if (property.propertyType is SerializedPropertyType.Float or SerializedPropertyType.Integer)
			{
				var timeField = new Vector3Field(property.displayName)
				{
					value = new Vector3(EditorPrefs.GetFloat(timeVectorSaveKeyX), EditorPrefs.GetFloat(timeVectorSaveKeyY), EditorPrefs.GetFloat(timeVectorSaveKeyZ)),
					tooltip = property.tooltip
				};

				timeField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				AddPropertyContextMenu(timeField, property);

				timeField.RegisterValueChangedCallback((callback) =>
				{
					switch (property.propertyType)
					{
						case SerializedPropertyType.Float:
							property.floatValue = GetConvertedTimeValue(callback.newValue, timeFieldAttribute);
							break;

						case SerializedPropertyType.Integer:
							property.intValue = Mathf.RoundToInt(GetConvertedTimeValue(callback.newValue, timeFieldAttribute));
							break;
					}

					EditorPrefs.SetFloat(timeVectorSaveKeyX, callback.newValue.x);
					EditorPrefs.SetFloat(timeVectorSaveKeyY, callback.newValue.y);
					EditorPrefs.SetFloat(timeVectorSaveKeyZ, callback.newValue.z);
					property.serializedObject.ApplyModifiedProperties();
				});

				root.Add(timeField);

				ExecuteLater(timeField, () =>
				{
					var labels = timeField.Query<Label>(className: TextInputBaseField<Void>.labelUssClassName).ToList();

					foreach (var label in labels)
					{
						label.text = GetFormatInitial(labels.IndexOf(label), timeFieldAttribute);
						label.tooltip = GetFormatTooltip(labels.IndexOf(label), timeFieldAttribute);
					}
				});
			}
			else
			{
				errorBox.text = "The TimeField Attribute can only be attached to Integers or Floats";
			}

			DisplayErrorBox(root, errorBox);

			return root;
		}

		protected override string CopyValue(VisualElement element, SerializedProperty property)
		{
			var vector3field = element as Vector3Field;

			return $"Vector3{vector3field.value}";
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var vector3field = element as Vector3Field;

			vector3field.value = VectorUtils.ParseVector3(clipboardValue.Replace("Vector3", ""));
		}

		private string GetFormatTooltip(int index, TimeFieldAttribute timeFieldAttribute)
		{
			return timeFieldAttribute.TimeFormat switch
			{
				TimeFormat.YearMonthWeek => index switch
				{
					0 => "Year",
					1 => "Month",
					2 => "Week",
					_ => string.Empty,
				},
				TimeFormat.YearMonthDay => index switch
				{
					0 => "Year",
					1 => "Month",
					2 => "Day",
					_ => string.Empty,
				},
				TimeFormat.WeekDayHour => index switch
				{
					0 => "Week",
					1 => "Day",
					2 => "Hour",
					_ => string.Empty,
				},
				TimeFormat.DayHourMinute => index switch
				{
					0 => "Day",
					1 => "Hour",
					2 => "Minute",
					_ => string.Empty,
				},
				TimeFormat.HourMinuteSecond => index switch
				{
					0 => "Hour",
					1 => "Minute",
					2 => "Second",
					_ => string.Empty,
				},
				_ => string.Empty,
			};
		}

		private string GetFormatInitial(int index, TimeFieldAttribute timeFieldAttribute)
		{
			return timeFieldAttribute.TimeFormat switch
			{
				TimeFormat.YearMonthWeek => index switch
				{
					0 => "Y",
					1 => "M",
					2 => "W",
					_ => string.Empty,
				},
				TimeFormat.YearMonthDay => index switch
				{
					0 => "Y",
					1 => "M",
					2 => "D",
					_ => string.Empty,
				},
				TimeFormat.WeekDayHour => index switch
				{
					0 => "W",
					1 => "D",
					2 => "H",
					_ => string.Empty,
				},
				TimeFormat.DayHourMinute => index switch
				{
					0 => "D",
					1 => "H",
					2 => "M",
					_ => string.Empty,
				},
				TimeFormat.HourMinuteSecond => index switch
				{
					0 => "H",
					1 => "M",
					2 => "S",
					_ => string.Empty,
				},
				_ => string.Empty,
			};
		}

		private float GetConvertedTimeValue(Vector3 value, TimeFieldAttribute timeFieldAttribute)
		{
			double total = timeFieldAttribute.TimeFormat switch
			{
				TimeFormat.YearMonthWeek =>
					GetConverted(value.x, Unit.Year, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.y, Unit.Month, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.z, Unit.Week, timeFieldAttribute.ConversionUnit),

				TimeFormat.YearMonthDay =>
					GetConverted(value.x, Unit.Year, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.y, Unit.Month, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.z, Unit.Day, timeFieldAttribute.ConversionUnit),

				TimeFormat.WeekDayHour =>
					GetConverted(value.x, Unit.Week, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.y, Unit.Day, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.z, Unit.Hour, timeFieldAttribute.ConversionUnit),

				TimeFormat.DayHourMinute =>
					GetConverted(value.x, Unit.Day, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.y, Unit.Hour, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.z, Unit.Minute, timeFieldAttribute.ConversionUnit),

				TimeFormat.HourMinuteSecond =>
					GetConverted(value.x, Unit.Hour, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.y, Unit.Minute, timeFieldAttribute.ConversionUnit) +
					GetConverted(value.z, Unit.Second, timeFieldAttribute.ConversionUnit),

				_ => 0
			};

			return (float)total;
		}

		private double GetConverted(float value, Unit from, Unit to)
		{
			if (from == to)
				return value;

			return value * UnitConverter.GetConversion(from.ToString(), to.ToString()).conversion;
		}
	}
}
