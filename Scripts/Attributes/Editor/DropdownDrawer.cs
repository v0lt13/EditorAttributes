using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var dropdownAttribute = attribute as DropdownAttribute;

			var stringArrayProperty = property.serializedObject.FindProperty(dropdownAttribute.stringArrayName);

			if (stringArrayProperty != null && stringArrayProperty.isArray && property.propertyType == SerializedPropertyType.String)
			{
				int selectedIndex = 0;
				var stringArray = new string[stringArrayProperty.arraySize];

				for (int i = 0; i < stringArrayProperty.arraySize; i++)
				{
					var arrayElementProperty = stringArrayProperty.GetArrayElementAtIndex(i);
					stringArray[i] = arrayElementProperty.stringValue;

					if (arrayElementProperty.stringValue == property.stringValue) selectedIndex = i;
				}

				selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, stringArray);

				if (selectedIndex >= 0 && selectedIndex < stringArray.Length) property.stringValue = stringArray[selectedIndex];
			}
			else
			{
				EditorGUILayout.HelpBox("Could not find the string array or the attached property is not a string", MessageType.Warning);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label);
	}
}
