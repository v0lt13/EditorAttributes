using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
// using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
#pragma warning disable CS0618
    [CustomPropertyDrawer(typeof(ColorFieldAttribute))]
    public class ColorFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // var colorFieldAttribute = attribute as ColorFieldAttribute;

            PropertyField propertyField = CreatePropertyField(property);

            /*
			HelpBox errorBox = new();
			
			ColorUtils.ApplyColor(propertyField, colorFieldAttribute, errorBox);			
			DisplayErrorBox(propertyField, errorBox); 
			*/

            // Because UI Toolkit makes tinting elements very difficult, it's not possible to properly tint properties with ColorField, this attribute will stay deprecated until Unity adds an easier way of tinting elements

            /* UPDATE:
            * Unity has added in version 6.3 a tint filter but it is not working as expected as it fully colors input fields and covers all text.
            * An alternative is using a custom UI shader for UI Toolkit but it is only limited to URP for some reason and requires a Shader Graph asset which I can't add since this package supports Unity 6.0 
            * as well and importing the shader there would break it.
            * 
            * You can make use of ApplyMaterial Attribute and assign it a custom shader that tints UI Toolkit elements if you want to color fields properly in the inspector in Unity 6.3+
            */

            // If you think this attribute would work well enough for you, then feel free to uncomment all the code
            propertyField.Add(new HelpBox("This attribute has been deprecated, use <b>GUIColor</b> instead. See <b>ColorFieldDrawer.cs</b> for more details", HelpBoxMessageType.Warning));

            return propertyField;
        }
    }
}
