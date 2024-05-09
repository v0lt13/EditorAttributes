using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PropertyWidthAttribute))]
    public class PropertyWidthDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var propertyWidthAttribute = attribute as PropertyWidthAttribute;

			var root = new VisualElement();
			var propertyField = DrawProperty(property);

			// Query the label 1ms later so the visual tree is properly initialized else the query will fail and the label will be null
			root.schedule.Execute(() => propertyField.Q<Label>().style.marginRight = propertyWidthAttribute.WidthOffset).ExecuteLater(1);

			root.Add(propertyField);

			return root;
		}
    }
}
