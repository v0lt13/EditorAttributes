using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
    public class DisableInPlayModeDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (Application.isPlaying) GUI.enabled = false;

			DrawProperty(position, property, label);

			GUI.enabled = true;
		}
	}
}
