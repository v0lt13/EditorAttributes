using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadonlyDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var propertyField = new PropertyField(property);

			propertyField.SetEnabled(false);

			return propertyField;
		}
	}
}
