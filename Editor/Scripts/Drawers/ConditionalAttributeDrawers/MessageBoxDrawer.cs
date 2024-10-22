using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(MessageBoxAttribute))]
    public class MessageBoxDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var messageBoxAttribute = attribute as MessageBoxAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(messageBoxAttribute.ConditionName, property);

			var root = new VisualElement();
			var errorBox = new HelpBox();
			var messageBox = new HelpBox("", (HelpBoxMessageType)messageBoxAttribute.MessageType);

			if (CanApplyGlobalColor)
			{
				messageBox.style.color = EditorExtension.GLOBAL_COLOR;
				messageBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
			}

			UpdateVisualElement(root, () =>
			{
				if (GetConditionValue(conditionalProperty, messageBoxAttribute, property, errorBox))
				{
					messageBox.text = GetDynamicString(messageBoxAttribute.Message, property, messageBoxAttribute, errorBox);
					root.Add(messageBox);
				}
				else
				{
					RemoveElement(root, messageBox);
				}

				DisplayErrorBox(root, errorBox);
			});

			root.Add(DrawProperty(property));

			return root;
		}
	}
}
