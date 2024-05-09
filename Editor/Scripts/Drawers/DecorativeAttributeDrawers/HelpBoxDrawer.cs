using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : DecoratorDrawer
    {
		public override VisualElement CreatePropertyGUI()
		{
			var helpbox = attribute as HelpBoxAttribute;
			
			var root = new VisualElement();
			var helpBox = new HelpBox(helpbox.Message, (HelpBoxMessageType)helpbox.MessageType);

			if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
				helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;

			root.Add(helpBox);

			return root;
		}
	}
}
