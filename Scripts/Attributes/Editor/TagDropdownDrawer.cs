using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TagDropdownAttribute))]
    public class TagDropdownDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            if (property.propertyType == SerializedPropertyType.String)
            {
                property.stringValue = EditorGUI.TagField(position, label, DoesStringValueContainTag(property.stringValue) ? property.stringValue : "Untagged");
			}
			else
            {
				EditorGUILayout.HelpBox("The TagDropdown attribute can only be attached to string fields", MessageType.Error);
			}
    	}

        private bool DoesStringValueContainTag(string stringValue)
        {
            foreach (var tag in InternalEditorUtility.tags)
            {
                if (stringValue == tag) return true;
            }

            return false;
        }
    }
}
