using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    internal static class EditorHandles
    {
        internal static Dictionary<string, (SerializedProperty serializedProperty, DrawHandleAttribute drawHandleAttribute)> handleProperties = new();
        internal static Dictionary<string, BoxBoundsHandle> boundsHandleList = new();

        internal static void DrawHandles()
        {
            foreach (var value in handleProperties.Values)
            {
                SerializedProperty serializedProperty = value.serializedProperty;
                DrawHandleAttribute drawHandleAttribute = value.drawHandleAttribute;

                try
                {
                    const float labelPostionAdd = 0.3f;
                    var target = serializedProperty.serializedObject.targetObject as Component;

                    if (drawHandleAttribute.HandleSpace == Space.Self)
                        Handles.matrix = target.transform.localToWorldMatrix;

                    Handles.color = ColorUtils.ColorAttributeToColor(drawHandleAttribute);

                    switch (serializedProperty.propertyType)
                    {
                        case SerializedPropertyType.Integer:
                            serializedProperty.intValue = (int)Handles.RadiusHandle(Quaternion.identity, Vector3.zero, serializedProperty.intValue);
                            break;

                        case SerializedPropertyType.Float:
                            serializedProperty.floatValue = Handles.RadiusHandle(Quaternion.identity, Vector3.zero, serializedProperty.floatValue);
                            break;

                        case SerializedPropertyType.Vector2:
                            Vector2 positionVector2 = serializedProperty.vector2Value;
                            Vector3 handlePositionVector2 = Handles.PositionHandle(positionVector2, Quaternion.identity);

                            serializedProperty.vector2Value = handlePositionVector2;

                            Handles.Label(VectorUtils.AddVector(positionVector2, labelPostionAdd), serializedProperty.displayName, EditorStyles.boldLabel);
                            break;

                        case SerializedPropertyType.Vector3:
                            Vector3 positionVector3 = serializedProperty.vector3Value;
                            Vector3 handlePositionVector3 = Handles.PositionHandle(positionVector3, Quaternion.identity);

                            serializedProperty.vector3Value = handlePositionVector3;

                            Handles.Label(VectorUtils.AddVector(positionVector3, labelPostionAdd), serializedProperty.displayName, EditorStyles.boldLabel);
                            break;

                        case SerializedPropertyType.Vector2Int:
                            Vector2Int positionVector2Int = serializedProperty.vector2IntValue;
                            Vector3 handlePositionVector2Int = Handles.PositionHandle(VectorUtils.Vector2IntToVector2(positionVector2Int), Quaternion.identity);

                            serializedProperty.vector2IntValue = VectorUtils.Vector2ToVector2Int(handlePositionVector2Int);

                            Handles.Label(VectorUtils.AddVector(VectorUtils.Vector2IntToVector2(positionVector2Int), labelPostionAdd), serializedProperty.displayName, EditorStyles.boldLabel);
                            break;

                        case SerializedPropertyType.Vector3Int:
                            Vector3Int positionVector3Int = serializedProperty.vector3IntValue;
                            Vector3 handlePositionVector3Int = Handles.PositionHandle(positionVector3Int, Quaternion.identity);

                            serializedProperty.vector3IntValue = VectorUtils.Vector3ToVector3Int(handlePositionVector3Int);

                            Handles.Label(VectorUtils.AddVector(positionVector3Int, labelPostionAdd), serializedProperty.displayName, EditorStyles.boldLabel);
                            break;

                        case SerializedPropertyType.Bounds:
                            Bounds boundsValue = serializedProperty.boundsValue;

                            boundsHandleList.TryGetValue(serializedProperty.propertyPath, out BoxBoundsHandle boundsHandle);

                            Vector3 targetPosition = target.transform.position;
                            Quaternion targetRotation = target.transform.rotation;

                            boundsHandle.center = boundsValue.center;
                            boundsHandle.size = boundsValue.size;

                            boundsHandle.DrawHandle();

                            serializedProperty.boundsValue = new Bounds(boundsHandle.center, boundsHandle.size);
                            break;

                        case SerializedPropertyType.Generic: // SimpleTransform type
                            SimpleTransform transformValue = GetSimpleTransformValuesFromSerializedProperty(serializedProperty);
                            Vector3 positionValue = transformValue.position;
                            Quaternion rotationValue = transformValue.QuaternionRotation;

                            Handles.TransformHandle(ref positionValue, ref rotationValue, ref transformValue.scale);

                            transformValue.position = positionValue;
                            transformValue.rotation = rotationValue.eulerAngles;

                            Handles.Label(VectorUtils.AddVector(positionValue, labelPostionAdd), serializedProperty.displayName, EditorStyles.boldLabel);

                            SetSimpleTransformValueFromSerializedProperty(serializedProperty, transformValue);
                            break;
                    }

                    Handles.matrix = Matrix4x4.identity;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }
                catch (ObjectDisposedException)
                {
                    handleProperties.Remove(serializedProperty.propertyPath);
                    break;
                }
            }
        }

        private static SimpleTransform GetSimpleTransformValuesFromSerializedProperty(SerializedProperty property) => new()
        {
            position = property.FindPropertyRelative("position").vector3Value,
            rotation = property.FindPropertyRelative("rotation").vector3Value,
            scale = property.FindPropertyRelative("scale").vector3Value
        };

        private static void SetSimpleTransformValueFromSerializedProperty(SerializedProperty property, SimpleTransform value)
        {
            property.FindPropertyRelative("position").vector3Value = value.position;
            property.FindPropertyRelative("rotation").vector3Value = value.rotation;
            property.FindPropertyRelative("scale").vector3Value = value.scale;
        }
    }
}
