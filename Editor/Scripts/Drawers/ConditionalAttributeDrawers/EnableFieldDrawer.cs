using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(EnableFieldAttribute))]
    public class EnableFieldDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var enableAttribute = attribute as EnableFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(enableAttribute.ConditionName, property);

			var root = new VisualElement();
			var errorBox = new HelpBox();

			var propertyField = DrawProperty(property);

			UpdateVisualElement(root, () =>
			{
				propertyField.SetEnabled(GetConditionValue(conditionalProperty, enableAttribute, property, errorBox));

				DisplayErrorBox(root, errorBox);
			});

			root.Add(propertyField);

			return root;
		}
	}
}
