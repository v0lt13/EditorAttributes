using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ShowFieldAttribute))]
    public class ShowFieldDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var showAttribute = attribute as ShowFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(showAttribute.ConditionName, property);

			var root = new VisualElement();
			var errorBox = new HelpBox();

			var propertyField = DrawProperty(property);

			UpdateVisualElement(root, () =>
			{
				if (GetConditionValue(conditionalProperty, showAttribute, property, errorBox))
				{
					root.Add(propertyField);
				}
				else
				{
					RemoveElement(root, propertyField);
				}

				DisplayErrorBox(root, errorBox);
			});

			root.Add(propertyField);

			return root;
		}
	}
}
