using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
    public class DisableInPlayModeDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement propertyField = CreatePropertyField(property);
            propertyField.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);

            return propertyField;
        }
    }
}
