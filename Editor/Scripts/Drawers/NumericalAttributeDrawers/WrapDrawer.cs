using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(WrapAttribute))]
    public class WrapDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var wrapAttribute = attribute as WrapAttribute;

			var minMaxX = (wrapAttribute.MinValueX, wrapAttribute.MaxValueX);
			var minMaxY = (wrapAttribute.MinValueY, wrapAttribute.MaxValueY);
			var minMaxZ = (wrapAttribute.MinValueZ, wrapAttribute.MaxValueZ);
			var minMaxW = (wrapAttribute.MinValueW, wrapAttribute.MaxValueW);

			var root = new VisualElement();

			var propertyField = DrawProperty(property);
			var helpBox = new HelpBox("", HelpBoxMessageType.Warning);

			root.RegisterCallback<SerializedPropertyChangeEvent>((e) =>
			{
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						property.intValue = (int)Wrap(property.intValue, minMaxX.MinValueX, minMaxX.MaxValueX);
						propertyField.Q<IntegerField>().SetValueWithoutNotify(property.intValue);
						break;

					case SerializedPropertyType.Float:
						property.floatValue = Wrap(property.floatValue, minMaxX.MinValueX, minMaxX.MaxValueX);
						propertyField.Q<FloatField>().SetValueWithoutNotify(property.floatValue);
						break;

					case SerializedPropertyType.Vector2:
						property.vector2Value = WrapVector(minMaxX, minMaxY, minMaxZ, minMaxW, property.vector2Value);
						propertyField.Q<Vector2Field>().SetValueWithoutNotify(property.vector2Value);
						break;

					case SerializedPropertyType.Vector2Int:
						property.vector2IntValue = VectorUtils.Vector3IntToVector2Int(WrapIntVector(minMaxX, minMaxY, minMaxZ, minMaxW, new(property.vector2IntValue.x, property.vector2IntValue.y)));
						propertyField.Q<Vector2IntField>().SetValueWithoutNotify(property.vector2IntValue);
						break;

					case SerializedPropertyType.Vector3:
						property.vector3Value = WrapVector(minMaxX, minMaxY, minMaxZ, minMaxW, property.vector3Value);
						propertyField.Q<Vector3Field>().SetValueWithoutNotify(property.vector3Value);
						break;

					case SerializedPropertyType.Vector3Int:
						property.vector3IntValue = WrapIntVector(minMaxX, minMaxY, minMaxZ, minMaxW, property.vector3IntValue);
						propertyField.Q<Vector3IntField>().SetValueWithoutNotify(property.vector3IntValue);
						break;

					case SerializedPropertyType.Rect:
						property.rectValue = WrapRect(minMaxX, minMaxY, minMaxZ, minMaxW, property.rectValue);
						propertyField.Q<RectField>().SetValueWithoutNotify(property.rectValue);
						break;

					case SerializedPropertyType.RectInt:
						property.rectIntValue = WrapIntRect(minMaxX, minMaxY, minMaxZ, minMaxW, property.rectIntValue);
						propertyField.Q<RectIntField>().SetValueWithoutNotify(property.rectIntValue);
						break;

					case SerializedPropertyType.Vector4:
						helpBox.text = "Vector4's are not supported";
						root.Add(helpBox);
						return;

					case SerializedPropertyType.Quaternion:
						helpBox.text = "Quaternions are not supported because they are weird";
						root.Add(helpBox);
						return;

					default:
						helpBox.text = "The attached field must be numerical";
						root.Add(helpBox);
						return;
				}
			});

			root.Add(propertyField);

			return root;
		}

		private Vector4 WrapVector((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, Vector4 vectorValue)
		{
			WrapAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vectorValue);

			return vectorValue;
		}

		private Rect WrapRect((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, Rect rectValue)
		{
			var vector4 = new Vector4(rectValue.x, rectValue.y, rectValue.width, rectValue.height);

			WrapAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vector4);

			return new Rect(vector4.x, vector4.y, vector4.z, vector4.w);
		}

		private Vector3Int WrapIntVector((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, Vector3Int vectorValue)
		{
			var vector4 = new Vector4(vectorValue.x, vectorValue.y, vectorValue.z);

			WrapAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vector4);

			return new Vector3Int((int)vector4.x, (int)vector4.y, (int)vector4.z);
		}

		private RectInt WrapIntRect((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, RectInt rectValue)
		{
			var vector4 = new Vector4(rectValue.x, rectValue.y, rectValue.width, rectValue.height);

			WrapAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vector4);

			return new RectInt((int)vector4.x, (int)vector4.y, (int)vector4.z, (int)vector4.w);
		}

		private void WrapAxis((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, ref Vector4 vector)
		{
			var x = Wrap(vector.x, minMaxX.Item1, minMaxX.Item2);
			var y = Wrap(vector.y, minMaxY.Item1, minMaxY.Item2);
			var z = Wrap(vector.z, minMaxZ.Item1, minMaxZ.Item2);
			var w = Wrap(vector.w, minMaxW.Item1, minMaxW.Item2);

			vector = new Vector4(x, y, z, w);
		}

		private float Wrap(float value, float min, float max)
		{
			if (value > max) value = min;

			if (value < min) value = max;

			return value;
		}
	}
}
