using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(HidePropertyAttribute))]
	public class HidePropertyDrawer : PropertyDrawerBase
	{
		public const string HIDDEN_PROPERTY_ID = "HiddenProperty";

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var propertyField = base.CreatePropertyGUI(property);

			propertyField.name = HIDDEN_PROPERTY_ID; // This is an identifier so that groups can make the field visible
			propertyField.style.display = DisplayStyle.None;

			return propertyField;
		}
	}
}
