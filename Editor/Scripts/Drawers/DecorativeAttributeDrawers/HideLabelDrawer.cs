using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(HideLabelAttribute))]
	public class HideLabelDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var propertyField = CreatePropertyField(property);

			ExecuteLater(propertyField, () =>
			{
				var label = propertyField.Q<Label>(className: "unity-label");

				if (label != null)
					label.style.display = DisplayStyle.None;
			});

			return propertyField;
		}
	}
}
