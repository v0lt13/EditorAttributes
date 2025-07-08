using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(UnitFieldAttribute))]
	public class UnitFieldDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var unitFieldAttribute = attribute as UnitFieldAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();

			var convertedUnit = UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);

			if (convertedUnit == null)
			{
				errorBox.text = $"No conversion found for <b>{unitFieldAttribute.DisplayUnit}</b> to <b>{unitFieldAttribute.ConversionUnit}</b>. You can add custom conversions in the <b>ProjectSettings/EditorAttributes</b> window";

				DisplayErrorBox(root, errorBox);
				return root;
			}

			if (IsSupportedType(property.propertyType))
			{
				var propertyValue = GetConvertedPropertyValue(property, convertedUnit.conversion);
				var numericType = GetFieldNumericType(property);

				var numericField = CreateFieldForType(numericType, property.displayName, propertyValue, property.hasMultipleDifferentValues);

				numericField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

				RegisterValueChangedCallbackByType(numericType, numericField, (value) =>
				{
					convertedUnit = UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);

					SetNumericPropertyValue(property, value, convertedUnit.conversion);
				});

				root.Add(numericField);

				ExecuteLater(numericField, () =>
				{
					var inputLabels = numericField.Query<VisualElement>(TextInputBaseField<Void>.textInputUssName).ToList();

					foreach (var inputLabel in inputLabels)
					{
						var unitLabel = new Label(convertedUnit.unitLabel)
						{
							tooltip = unitFieldAttribute.DisplayUnit,
							style =
							{
								unityTextAlign = TextAnchor.MiddleRight,
								color = Color.gray,
							}
						};

						inputLabel.Add(unitLabel);
					}
				});
			}
			else
			{
				errorBox.text = "The UnitField Attribute can only be attached to numeric types";
			}

			DisplayErrorBox(root, errorBox);

			return root;
		}

		private Type GetFieldNumericType(SerializedProperty property) => property.numericType switch
		{
			SerializedPropertyNumericType.Int32 => typeof(int),
			SerializedPropertyNumericType.Int64 => typeof(long),
			SerializedPropertyNumericType.UInt32 => typeof(uint),
			SerializedPropertyNumericType.UInt64 => typeof(ulong),
			SerializedPropertyNumericType.Float => typeof(float),
			SerializedPropertyNumericType.Double => typeof(double),
			_ => property.propertyType switch
			{
				SerializedPropertyType.Vector2 => typeof(Vector2),
				SerializedPropertyType.Vector3 => typeof(Vector3),
				SerializedPropertyType.Vector4 => typeof(Vector4),
				SerializedPropertyType.Rect => typeof(Rect),
				SerializedPropertyType.Vector2Int => typeof(Vector2Int),
				SerializedPropertyType.Vector3Int => typeof(Vector3Int),
				SerializedPropertyType.RectInt => typeof(RectInt),
				_ => null,
			}
		};

		private object GetConvertedPropertyValue(SerializedProperty property, double conversion)
		{
			// Invert the conversion factor to go from base -> converted unit
			double displayFactor = 1.0d / conversion;

			return property.numericType switch
			{
				SerializedPropertyNumericType.Float => property.floatValue * (float)displayFactor,
				SerializedPropertyNumericType.Double => property.doubleValue * displayFactor,
				SerializedPropertyNumericType.Int32 => (int)(property.intValue * displayFactor),
				SerializedPropertyNumericType.Int64 => (long)(property.longValue * displayFactor),
				SerializedPropertyNumericType.UInt32 => (uint)(property.uintValue * displayFactor),
				SerializedPropertyNumericType.UInt64 => (ulong)(property.ulongValue * displayFactor),

				_ => property.propertyType switch
				{
					SerializedPropertyType.Vector2 => new Vector2(
						property.vector2Value.x * (float)displayFactor,
						property.vector2Value.y * (float)displayFactor
					),

					SerializedPropertyType.Vector3 => new Vector3(
						property.vector3Value.x * (float)displayFactor,
						property.vector3Value.y * (float)displayFactor,
						property.vector3Value.z * (float)displayFactor
					),

					SerializedPropertyType.Vector4 => new Vector4(
						property.vector4Value.x * (float)displayFactor,
						property.vector4Value.y * (float)displayFactor,
						property.vector4Value.z * (float)displayFactor,
						property.vector4Value.w * (float)displayFactor
					),

					SerializedPropertyType.Rect => new Rect(
						property.rectValue.x * (float)displayFactor,
						property.rectValue.y * (float)displayFactor,
						property.rectValue.width * (float)displayFactor,
						property.rectValue.height * (float)displayFactor
					),

					SerializedPropertyType.Vector2Int => new Vector2Int(
						(int)(property.vector2IntValue.x * displayFactor),
						(int)(property.vector2IntValue.y * displayFactor)
					),

					SerializedPropertyType.Vector3Int => new Vector3Int(
						(int)(property.vector3IntValue.x * displayFactor),
						(int)(property.vector3IntValue.y * displayFactor),
						(int)(property.vector3IntValue.z * displayFactor)
					),

					SerializedPropertyType.RectInt => new RectInt(
						(int)(property.rectIntValue.x * displayFactor),
						(int)(property.rectIntValue.y * displayFactor),
						(int)(property.rectIntValue.width * displayFactor),
						(int)(property.rectIntValue.height * displayFactor)
					),
					_ => null
				}
			};
		}

		private void SetNumericPropertyValue(SerializedProperty property, object value, double conversion)
		{
			double convertedValue = 0d;

			try
			{
				convertedValue = Convert.ToDouble(value) * conversion;
			}
			catch (InvalidCastException) { } // If this is thrown then it means that the value is a vector or rect type, which we handle separately

			switch (property.numericType)
			{
				case SerializedPropertyNumericType.Float:
					property.floatValue = (float)convertedValue;
					break;

				case SerializedPropertyNumericType.Double:
					property.doubleValue = convertedValue;
					break;

				case SerializedPropertyNumericType.Int32:
					property.intValue = (int)convertedValue;
					break;

				case SerializedPropertyNumericType.Int64:
					property.longValue = (long)convertedValue;
					break;

				case SerializedPropertyNumericType.UInt32:
					property.uintValue = (uint)convertedValue;
					break;

				case SerializedPropertyNumericType.UInt64:
					property.ulongValue = (ulong)convertedValue;
					break;

				default:
					switch (property.propertyType)
					{
						case SerializedPropertyType.Vector2:
							var vector2Value = (Vector2)value;
							property.vector2Value = new Vector2(vector2Value.x * (float)conversion, vector2Value.y * (float)conversion);
							break;

						case SerializedPropertyType.Vector3:
							var vector3Value = (Vector3)value;
							property.vector3Value = new Vector3(vector3Value.x * (float)conversion, vector3Value.y * (float)conversion, vector3Value.z * (float)conversion);
							break;

						case SerializedPropertyType.Vector4:
							var vector4Value = (Vector4)value;
							property.vector4Value = new Vector4(vector4Value.x * (float)conversion, vector4Value.y * (float)conversion, vector4Value.z * (float)conversion, vector4Value.w * (float)conversion);
							break;

						case SerializedPropertyType.Rect:
							var rectValue = (Rect)value;
							property.rectValue = new Rect(rectValue.x * (float)conversion, rectValue.y * (float)conversion, rectValue.width * (float)conversion, rectValue.height * (float)conversion);
							break;

						case SerializedPropertyType.Vector2Int:
							var vector2IntValue = (Vector2Int)value;
							property.vector2IntValue = new Vector2Int((int)(vector2IntValue.x * conversion), (int)(vector2IntValue.y * conversion));
							break;

						case SerializedPropertyType.Vector3Int:
							var vector3IntValue = (Vector3Int)value;
							property.vector3IntValue = new Vector3Int((int)(vector3IntValue.x * conversion), (int)(vector3IntValue.y * conversion), (int)(vector3IntValue.z * conversion));
							break;

						case SerializedPropertyType.RectInt:
							var rectIntValue = (RectInt)value;
							property.rectIntValue = new RectInt((int)(rectIntValue.x * conversion), (int)(rectIntValue.y * conversion), (int)(rectIntValue.width * conversion), (int)(rectIntValue.height * conversion));
							break;
					}
					break;
			}

			property.serializedObject.ApplyModifiedProperties();
		}

		private bool IsSupportedType(SerializedPropertyType propertyType) => propertyType is SerializedPropertyType.Float or SerializedPropertyType.Integer
			or SerializedPropertyType.Vector2 or SerializedPropertyType.Vector3 or SerializedPropertyType.Vector4
			or SerializedPropertyType.Rect or SerializedPropertyType.RectInt
			or SerializedPropertyType.Vector2Int or SerializedPropertyType.Vector3Int;
	}
}
