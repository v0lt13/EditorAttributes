using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var dropdownAttribute = attribute as DropdownAttribute;

			var memberInfo = ReflectionUtility.GetValidMemberInfo(dropdownAttribute.ArrayName, property);
			var stringArray = GetArrayValues(property, memberInfo);

			int selectedIndex = 0;

			for (int i = 0; i < stringArray.Length; i++)
			{
				if (stringArray[i] == GetPropertyValueAsString(property)) selectedIndex = i;
			}

			selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, stringArray);

			if (selectedIndex >= 0 && selectedIndex < stringArray.Length) SetProperyValueAsString(stringArray[selectedIndex], ref property);
		}

		public string[] GetArrayValues(SerializedProperty serializedProperty, MemberInfo memberInfo)
		{
			var stringList = new List<string>();

			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType.IsArray || memberInfoType.GetInterfaces().Contains(typeof(IList)))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue is Array array)
				{
					foreach (var item in array) stringList.Add(item.ToString());
				}
				else if (memberInfoValue is IList list)
				{
					foreach (var item in list) stringList.Add(item.ToString());
				}

				return stringList.ToArray();
			}

			EditorGUILayout.HelpBox("Could not find the array or the attached property is not valid", MessageType.Error);

			return new string[0];
		}
	}
}
