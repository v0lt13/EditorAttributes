using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
		private float boxHeight;

		public override void OnGUI(Rect position)
		{
			var messageBox = attribute as HelpBoxAttribute;

			var helpBoxStyle = GUI.skin.GetStyle("HelpBox");
			helpBoxStyle.richText = true;

			boxHeight = messageBox.MessageType == MessageMode.None ? EditorGUIUtility.singleLineHeight : 40f;

			EditorGUI.HelpBox(new Rect(position.x, position.y, position.width, boxHeight), messageBox.Message, (MessageType)messageBox.MessageType);
		}

		public override float GetHeight() => boxHeight + 2f;
	}
}
