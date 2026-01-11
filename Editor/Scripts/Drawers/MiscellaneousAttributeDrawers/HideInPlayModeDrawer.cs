using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideInPlayModeAttribute))]
    public class HideInPlayModeDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyField = CreatePropertyField(property);
            propertyField.style.display = !EditorApplication.isPlayingOrWillChangePlaymode ? DisplayStyle.Flex : DisplayStyle.None;

            return propertyField;
        }
    }
}
