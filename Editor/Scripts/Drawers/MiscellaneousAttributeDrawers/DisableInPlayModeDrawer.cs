using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableInPlayModeAttribute))]
    public class DisableInPlayModeDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			root.Add(new PropertyField(property));
			root.SetEnabled(!EditorApplication.isPlayingOrWillChangePlaymode);

			return root;
		}
	}
}
