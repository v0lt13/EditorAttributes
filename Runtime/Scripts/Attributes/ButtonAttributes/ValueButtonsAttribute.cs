using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to display a collection of values in toggleble buttons
	/// </summary>
	public class ValueButtonsAttribute : PropertyAttribute
	{
		public bool ShowLabel { get; private set; }
		public float ButtonsHeight { get; private set; }
		public string CollectionName { get; private set; }
		public string[] DisplayNames { get; private set; }

		/// <summary>
		/// Attribute to display a collection of values in toggleble buttons
		/// </summary>
		/// <param name="collectionName">The name of the collection</param>
		/// <param name="buttonsHeight">The height of the selection buttons in pixels</param>
		/// <param name="showLabel">Show the label of the field</param>
		public ValueButtonsAttribute(string collectionName, float buttonsHeight = 18f, bool showLabel = true)
		{
			CollectionName = collectionName;
			ButtonsHeight = buttonsHeight;
			ShowLabel = showLabel;
		}

		/// <summary>
		/// Attribute to display a collection of values in toggleble buttons
		/// </summary>
		/// <param name="collectionName">The name of the collection</param>
		/// <param name="displayNames">Change the display name for each button</param>
		/// <param name="buttonsHeight">The height of the selection buttons in pixels</param>
		/// <param name="showLabel">Show the label of the field</param>
		public ValueButtonsAttribute(string collectionName, string[] displayNames, float buttonsHeight = 18f, bool showLabel = true)
		{
			CollectionName = collectionName;
			DisplayNames = displayNames;
			ButtonsHeight = buttonsHeight;
			ShowLabel = showLabel;
		}
	}
}
