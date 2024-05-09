using UnityEditor;
using UnityEngine.UIElements;
//using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
#pragma warning disable CS0618
	[CustomPropertyDrawer(typeof(ColorFieldAttribute))]
	public class ColorFieldDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			//var colorFieldAttribute = attribute as ColorFieldAttribute;
			
			var root = new VisualElement();
			var propertyField = DrawProperty(property);
			
			/*
			var errorBox = new HelpBox();
			
			ColorUtility.ApplyColor(root, colorFieldAttribute, errorBox);
			
			DisplayErrorBox(root, errorBox); 
			*/

			root.Add(propertyField);

			// Because UI toolkit makes tinting elements very difficult, it's not possible to properly tint properties with ColorField, this attribute will stay deprecated until Unity adds an easyer way of tinting elements
			// Which I estimate it will be in about 234287346598175492164 years
			// If you think this attribute would work well enough for you, feel free to uncomment all the code
			root.Add(new HelpBox("This attribute has been deprecated, use GUIColor instead. See ColorFieldDrawer.cs for more details", HelpBoxMessageType.Warning));

			return root;
		}
	}
}
