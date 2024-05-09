using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TimeFieldAttribute))]
    public class TimeFieldDrawer : PropertyDrawerBase
    {
    	public override VisualElement CreatePropertyGUI(SerializedProperty property)
    	{
            var timeFieldAttribute = attribute as TimeFieldAttribute;

			var timeVectorSaveKeyX = $"{property.serializedObject.targetObject}_{property.propertyPath}_TimeX";
			var timeVectorSaveKeyY = $"{property.serializedObject.targetObject}_{property.propertyPath}_TimeY";
			var timeVectorSaveKeyZ = $"{property.serializedObject.targetObject}_{property.propertyPath}_TimeZ";

			var root = new VisualElement();
            var errorBox = new HelpBox();

            if (property.propertyType is SerializedPropertyType.Float or SerializedPropertyType.Integer)
            {
				var timeField = new Vector3Field(property.displayName)
				{
					value = new Vector3(EditorPrefs.GetFloat(timeVectorSaveKeyX), EditorPrefs.GetFloat(timeVectorSaveKeyY), EditorPrefs.GetFloat(timeVectorSaveKeyZ))
				};

                root.schedule.Execute(() =>
                {
                    var labels = timeField.Query<Label>(className: "unity-base-text-field__label").ToList();

                    foreach (var label in labels)
                        label.text = GetFormatInitial(labels.IndexOf(label), timeFieldAttribute);
                }).ExecuteLater(1);

				timeField.AddToClassList("unity-base-field__aligned");

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
            }
            else
            {
                errorBox.text = "The TimeField Attribute can only be attached to Integers or Floats";
            }

            DisplayErrorBox(root, errorBox);

            return root;
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
					_ => "ERR",
				},
				TimeFormat.YearMonthDay => index switch
				{
					0 => "Y",
					1 => "M",
					2 => "D",
					_ => "ERR",
				},
				TimeFormat.WeekDayHour => index switch
				{
					0 => "Y",
					1 => "D",
					2 => "H",
					_ => "ERR",
				},
				TimeFormat.DayHourMinute => index switch
				{
					0 => "D",
					1 => "H",
					2 => "M",
					_ => "ERR",
				},
				TimeFormat.HourMinuteSecond => index switch
				{
					0 => "H",
					1 => "M",
					2 => "S",
					_ => "ERR",
				},
				_ => "ERR",
			};
		}

		private float GetConvertedTimeValue(Vector3 value, TimeFieldAttribute timeFieldAttribute)
		{
			return timeFieldAttribute.TimeFormat switch
			{
				TimeFormat.YearMonthWeek => timeFieldAttribute.ConvertTo switch
				{
					ConvertTo.Days => value.x * 365f + value.y * 30f + value.z * 7f,
					ConvertTo.Hours => (value.x * 365f + value.y * 30f + value.z * 7f) * 24f,
					ConvertTo.Minutes => (value.x * 365f + value.y * 30f + value.z * 7f) * 24f * 60f,
					ConvertTo.Seconds => (value.x * 365f + value.y * 30f + value.z * 7f) * 24f * 60f * 60f,
					ConvertTo.Miliseconds => (value.x * 365f + value.y * 30f + value.z * 7f) * 24f * 60f * 60f * 1000f,
					_ => 0f
				},
				TimeFormat.YearMonthDay => timeFieldAttribute.ConvertTo switch
				{
					ConvertTo.Days => value.x * 365f + value.y * 30f + value.z,
					ConvertTo.Hours => (value.x * 365f + value.y * 30f + value.z) * 24f,
					ConvertTo.Minutes => (value.x * 365f + value.y * 30f + value.z) * 24f * 60f,
					ConvertTo.Seconds => (value.x * 365f + value.y * 30f + value.z) * 24f * 60f * 60f,
					ConvertTo.Miliseconds => (value.x * 365f + value.y * 30f + value.z) * 24f * 60f * 60f * 1000f,
					_ => 0f
				},
				TimeFormat.WeekDayHour => timeFieldAttribute.ConvertTo switch
				{
					ConvertTo.Days => value.x * 7f + value.y,
					ConvertTo.Hours => (value.x * 7f + value.y) * 24f + value.z,
					ConvertTo.Minutes => ((value.x * 7f + value.y) * 24f + value.z) * 60f,
					ConvertTo.Seconds => ((value.x * 7f + value.y) * 24f + value.z) * 60f * 60f,
					ConvertTo.Miliseconds => ((value.x * 7f + value.y) * 24f + value.z) * 60f * 60f * 1000f,
					_ => 0f
				},
				TimeFormat.DayHourMinute => timeFieldAttribute.ConvertTo switch
				{
					ConvertTo.Days => value.x + value.y / 24f + value.z / (24f * 60f),
					ConvertTo.Hours => value.x * 24f + value.y + value.z / 60f,
					ConvertTo.Minutes => (value.x * 24f * 60f) + value.y * 60f + value.z,
					ConvertTo.Seconds => ((value.x * 24f * 60f) + value.y * 60f + value.z) * 60f,
					ConvertTo.Miliseconds => ((value.x * 24f * 60f) + value.y * 60f + value.z) * 60f * 1000f,
					_ => 0f
				},
				TimeFormat.HourMinuteSecond => timeFieldAttribute.ConvertTo switch
				{
					ConvertTo.Days => value.x / 24f + value.y / (24f * 60f) + value.z / (24f * 60f * 60f),
					ConvertTo.Hours => value.x + value.y / 60f + value.z / (60f * 60f),
					ConvertTo.Minutes => (value.x * 60f) + value.y + value.z / 60f,
					ConvertTo.Seconds => ((value.x * 60f) + value.y) * 60f + value.z,
					ConvertTo.Miliseconds => (((value.x * 60f) + value.y) * 60f + value.z) * 1000f,
					_ => 0f
				},
				_ => 0f
			};
		}
    }
}
