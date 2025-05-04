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
			var messageBox = new HelpBox(string.Empty, (HelpBoxMessageType)messageBoxAttribute.MessageType);
			var errorBox = new HelpBox();

			var propertyField = CreatePropertyField(property);

			if (CanApplyGlobalColor)
			{
				messageBox.style.color = EditorExtension.GLOBAL_COLOR;
				messageBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
			}

			root.Add(propertyField);

			UpdateVisualElement(propertyField, () =>
			{
				if (GetConditionValue(conditionalProperty, messageBoxAttribute, property, errorBox))
				{
					messageBox.text = GetDynamicString(messageBoxAttribute.Message, property, messageBoxAttribute, errorBox);
			
					AddElement(root, messageBox);
			
					if (messageBoxAttribute.DrawAbove)
						messageBox.PlaceBehind(propertyField);
				}
				else
				{
					RemoveElement(root, messageBox);
				}
			
				DisplayErrorBox(root, errorBox);
			});

			return root;
		}
	}
}
