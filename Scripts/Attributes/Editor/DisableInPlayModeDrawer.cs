using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
    public class DisableInPlayModeDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			if (Application.isPlaying) GUI.enabled = false;

			try
			{
				eventDrawer.OnGUI(position, property, label);
			}
			catch (NullReferenceException)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			GUI.enabled = true;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			try
			{
				return eventDrawer.GetPropertyHeight(property, label);
			}
			catch (NullReferenceException)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
		}
	}
}
