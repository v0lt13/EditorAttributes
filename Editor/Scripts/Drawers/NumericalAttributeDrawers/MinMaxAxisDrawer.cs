using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    public abstract class MinMaxAxisDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var clampAttribute = attribute as IMinMaxAxisValueAttribute;

            var minMaxX = (clampAttribute.MinValueX, clampAttribute.MaxValueX);
            var minMaxY = (clampAttribute.MinValueY, clampAttribute.MaxValueY);
            var minMaxZ = (clampAttribute.MinValueZ, clampAttribute.MaxValueZ);
            var minMaxW = (clampAttribute.MinValueW, clampAttribute.MaxValueW);

            PropertyField propertyField = CreatePropertyField(property);

            propertyField.RegisterCallback<SerializedPropertyChangeEvent>((callback) =>
            {
                switch (property.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        switch (property.numericType)
                        {
                            default:
                            case SerializedPropertyNumericType.Int32:
                                property.intValue = (int)MinMaxValue(minMaxX, property.intValue);
                                propertyField.Q<IntegerField>().SetValueWithoutNotify(property.intValue);
                                break;

                            case SerializedPropertyNumericType.UInt32:
                                property.uintValue = (uint)MinMaxValue(minMaxX, property.uintValue);
                                propertyField.Q<UnsignedIntegerField>().SetValueWithoutNotify(property.uintValue);
                                break;

                            case SerializedPropertyNumericType.Int64:
                                property.longValue = (long)MinMaxValue(minMaxX, property.longValue);
                                propertyField.Q<LongField>().SetValueWithoutNotify(property.longValue);
                                break;

                            case SerializedPropertyNumericType.UInt64:
                                property.ulongValue = (ulong)MinMaxValue(minMaxX, property.ulongValue);
                                propertyField.Q<UnsignedLongField>().SetValueWithoutNotify(property.ulongValue);
                                break;
                        }
                        break;

                    case SerializedPropertyType.Float:
                        switch (property.numericType)
                        {
                            case SerializedPropertyNumericType.Float:
                                property.floatValue = MinMaxValue(minMaxX, property.floatValue);
                                propertyField.Q<FloatField>().SetValueWithoutNotify(property.floatValue);
                                break;

                            case SerializedPropertyNumericType.Double:
                                property.doubleValue = MinMaxValue(minMaxX, (float)property.doubleValue);
                                propertyField.Q<DoubleField>().SetValueWithoutNotify(property.doubleValue);
                                break;
                        }
                        break;

                    case SerializedPropertyType.Vector2:
                        property.vector2Value = MinMaxVector(minMaxX, minMaxY, minMaxZ, minMaxW, property.vector2Value);
                        propertyField.Q<Vector2Field>().SetValueWithoutNotify(property.vector2Value);
                        break;

                    case SerializedPropertyType.Vector2Int:
                        property.vector2IntValue = VectorUtils.Vector3IntToVector2Int(MinMaxIntVector(minMaxX, minMaxY, minMaxZ, new(property.vector2IntValue.x, property.vector2IntValue.y)));
                        propertyField.Q<Vector2IntField>().SetValueWithoutNotify(property.vector2IntValue);
                        break;

                    case SerializedPropertyType.Vector3:
                        property.vector3Value = MinMaxVector(minMaxX, minMaxY, minMaxZ, minMaxW, property.vector3Value);
                        propertyField.Q<Vector3Field>().SetValueWithoutNotify(property.vector3Value);
                        break;

                    case SerializedPropertyType.Vector3Int:
                        property.vector3IntValue = MinMaxIntVector(minMaxX, minMaxY, minMaxZ, property.vector3IntValue);
                        propertyField.Q<Vector3IntField>().SetValueWithoutNotify(property.vector3IntValue);
                        break;

                    case SerializedPropertyType.Rect:
                        property.rectValue = MinMaxRect(minMaxX, minMaxY, minMaxZ, minMaxW, property.rectValue);
                        propertyField.Q<RectField>().SetValueWithoutNotify(property.rectValue);
                        break;

                    case SerializedPropertyType.RectInt:
                        property.rectIntValue = MinMaxIntRect(minMaxX, minMaxY, minMaxZ, minMaxW, property.rectIntValue);
                        propertyField.Q<RectIntField>().SetValueWithoutNotify(property.rectIntValue);
                        break;

                    case SerializedPropertyType.Vector4:
                        propertyField.Add(new HelpBox("Vector4's are not supported", HelpBoxMessageType.Warning));
                        return;

                    case SerializedPropertyType.Quaternion:
                        propertyField.Add(new HelpBox("Quaternions are not supported because they are weird", HelpBoxMessageType.Warning));
                        return;

                    default:
                        propertyField.Add(new HelpBox("The attached field must be numerical", HelpBoxMessageType.Warning));
                        return;
                }

                property.serializedObject.ApplyModifiedProperties();
            });

            return propertyField;
        }

        protected abstract void MinMaxAxis((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, ref Vector4 vector);

        private float MinMaxValue((float, float) minMax, float value)
        {
            Vector4 vector4 = new(value, value);

            MinMaxAxis(minMax, (0f, 0f), (0f, 0f), (0f, 0f), ref vector4);

            return vector4.x;
        }

        private Vector4 MinMaxVector((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, Vector4 vectorValue)
        {
            MinMaxAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vectorValue);
            return vectorValue;
        }

        private Rect MinMaxRect((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, Rect rectValue)
        {
            Vector4 vector4 = new(rectValue.x, rectValue.y, rectValue.width, rectValue.height);

            MinMaxAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vector4);
            return new Rect(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        private Vector3Int MinMaxIntVector((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, Vector3Int vectorValue)
        {
            Vector4 vector4 = new(vectorValue.x, vectorValue.y, vectorValue.z);

            MinMaxAxis(minMaxX, minMaxY, minMaxZ, (0f, 0f), ref vector4);
            return new Vector3Int((int)vector4.x, (int)vector4.y, (int)vector4.z);
        }

        private RectInt MinMaxIntRect((float, float) minMaxX, (float, float) minMaxY, (float, float) minMaxZ, (float, float) minMaxW, RectInt rectValue)
        {
            Vector4 vector4 = new(rectValue.x, rectValue.y, rectValue.width, rectValue.height);

            MinMaxAxis(minMaxX, minMaxY, minMaxZ, minMaxW, ref vector4);
            return new RectInt((int)vector4.x, (int)vector4.y, (int)vector4.z, (int)vector4.w);
        }
    }
}
