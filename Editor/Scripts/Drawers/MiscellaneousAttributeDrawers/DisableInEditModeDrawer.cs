using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInEditModeAttribute))]
    public class DisableInEditModeDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement propertyField = CreatePropertyField(property);
            propertyField.SetEnabled(EditorApplication.isPlayingOrWillChangePlaymode);

            return propertyField;
        }
    }
}
