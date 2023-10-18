using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideInPlayModeAttribute))]
    public class HideInPlayModeDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			if (!Application.isPlaying)
			{
				try
				{
					eventDrawer.OnGUI(position, property, label);
				}
				catch (NullReferenceException)
				{
					EditorGUI.PropertyField(position, property, label, true);
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			if (!Application.isPlaying)
			{
				try
				{
					return eventDrawer.GetPropertyHeight(property, label);
				}
				catch (NullReferenceException)
				{
					return EditorGUI.GetPropertyHeight(property, label);
				}
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
			}
		}
	}
}
