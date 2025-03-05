using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInEditModeAttribute))]
    public class DisableInEditModeDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			root.Add(new PropertyField(property));
			root.SetEnabled(EditorApplication.isPlayingOrWillChangePlaymode);

			return root;
		}
	}
}
