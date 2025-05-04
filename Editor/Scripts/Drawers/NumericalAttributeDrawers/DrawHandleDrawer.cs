using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.IMGUI.Controls;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DrawHandleAttribute))]
    public class DrawHandleDrawer : PropertyDrawerBase
    {
    	public override VisualElement CreatePropertyGUI(SerializedProperty property)
    	{
    		var root = new VisualElement();
            var drawHandleAttribute = attribute as DrawHandleAttribute;

			root.Add(CreatePropertyField(property));

			switch (property.propertyType)
            {
                case SerializedPropertyType.Float: case SerializedPropertyType.Integer:
				case SerializedPropertyType.Vector2: case SerializedPropertyType.Vector2Int:
				case SerializedPropertyType.Vector3: case SerializedPropertyType.Vector3Int:
				case SerializedPropertyType.Bounds: case SerializedPropertyType.Generic:

					if (property.serializedObject.targetObject is not Component)
					{
						root.Add(new HelpBox("The DrawHandle Attribute can only be used with GameObjects", HelpBoxMessageType.Error));
                        break;
					}

                    if (property.propertyType == SerializedPropertyType.Generic && property.type != "SimpleTransform")
                        goto default;

                    if (drawHandleAttribute.HandleSpace == Space.Self)
                    {
                        if (property.propertyType == SerializedPropertyType.Vector2Int || property.propertyType == SerializedPropertyType.Vector3Int)
							root.Add(new HelpBox("Vector2Int and Vector3Int handles don't support local space", HelpBoxMessageType.Warning));
					}

                    if (!EditorHandles.handleProperties.ContainsKey(property.propertyPath))
                    {
					    EditorHandles.handleProperties.Add(property.propertyPath, (property, drawHandleAttribute));

                        if (property.propertyType == SerializedPropertyType.Bounds)
                            EditorHandles.boundsHandleList.Add(property.propertyPath, new BoxBoundsHandle());
                    }

					break;

                default:
                    root.Add(new HelpBox("The DrawHandle Attribute can only be used on int, float, Vector2, Vector2Int, Vector3, Vector3Int, Bounds and SimpleTransform fields", HelpBoxMessageType.Error));
                    break;
            }

    		return root;
    	}
    }
}
