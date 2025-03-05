using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var disableAttribute = attribute as DisableFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(disableAttribute.ConditionName, property);

			var root = new VisualElement();
			var errorBox = new HelpBox();

			var propertyField = new PropertyField(property);

			root.Add(propertyField);

			UpdateVisualElement(root, () => 
			{
				propertyField.SetEnabled(!GetConditionValue(conditionalProperty, disableAttribute, property, errorBox));

				DisplayErrorBox(root, errorBox);
			});

			return root;
		}
	}
}
