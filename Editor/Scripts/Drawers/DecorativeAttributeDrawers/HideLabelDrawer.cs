using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) => new PropertyField(property, string.Empty);
	}
}
