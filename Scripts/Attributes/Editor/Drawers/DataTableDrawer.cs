using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DataTableAttribute))]
    public class DataTableDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                EditorGUI.HelpBox(position, "The RowAttribute can only be attached to serialized structs or classes and collections containing them", MessageType.Error);
                return;
            }

			property.isExpanded = true;

			int childCount = property.Copy().CountInProperty() - 1;
			int initialDepth = property.depth;
			float currentX = position.x;

			var labelRect = new Rect(currentX, position.y, EditorGUIUtility.labelWidth, position.height);
			EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);

			currentX += labelRect.width + EditorGUIUtility.standardVerticalSpacing;

			while (property.NextVisible(true) && property.depth > initialDepth)
			{
				float propertyFieldWidth = Mathf.Max((position.width - EditorGUIUtility.labelWidth) / childCount, EditorGUIUtility.fieldWidth);

				if (property.propertyType == SerializedPropertyType.Generic || property.propertyType == SerializedPropertyType.Vector4 || property.propertyType == SerializedPropertyType.ArraySize)
				{
					var helpBoxRect = new Rect(currentX, position.y, propertyFieldWidth, position.height);

					EditorGUI.HelpBox(helpBoxRect, "Collection, UnityEvent and Serialized object types are not supported", MessageType.Error);
					break;
				}

				var propertyLabelRect = new Rect(currentX - 20f, position.y, propertyFieldWidth, position.height / 2f);
				var propertyFieldRect = new Rect(currentX - 20f, position.y + position.height / 2f, propertyFieldWidth, position.height / 2f);

				EditorGUI.LabelField(propertyLabelRect, property.displayName);
				EditorGUI.PropertyField(propertyFieldRect, property, GUIContent.none);

				currentX += propertyFieldRect.width + EditorGUIUtility.standardVerticalSpacing;
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight * 2f;
	}
}
