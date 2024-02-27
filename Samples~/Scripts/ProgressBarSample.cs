using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/NumericalAttributes/progressbar.html")]
	public class ProgressBarSample : MonoBehaviour
	{
		[Header("ProgressBar Attribute:")]
		[SerializeField, Range(0f, 100f)] private float value;

		[SerializeField, ProgressBar] private int intBar;
		[SerializeField, ProgressBar(100f, 50f)] private float floatBar;

		void OnValidate()
		{
			intBar = (int)value;
			floatBar = value;
		}
	}
}
