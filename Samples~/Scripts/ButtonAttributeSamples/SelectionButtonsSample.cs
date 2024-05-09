using System;
using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/ButtonAttributes/selectionbuttons.html")]
	public class SelectionButtonsSample : MonoBehaviour
	{
		public enum States
		{
			State01,
			State02,
			State03
		}

		[Flags]
		public enum Flags
		{
			Flag01 = 0, 
			Flag02 = 1,
			Flag03 = 2,
			Flag04 = 4,
			Flag05 = 8
		}

		[Header("SelectionButtons Attribute:")]
		[SerializeField, SelectionButtons] private States states;
		[SerializeField, SelectionButtons(showLabel: false)] private Flags flags;
		[SerializeField, SelectionButtons(nameof(stringValues), buttonsHeight: 30f)] private string stringField;

		private string[] stringValues = new string[]
		{
			"Value01", "Value02", "Value03", "Value04", "Value05", "Value06"
		};
	}
}
