using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(HideInChildrenAttribute))]
	public class HideInChildrenDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var hideInChildrenAttribute = attribute as HideInChildrenAttribute;
			var root = new VisualElement();

			if (!IsPropertyInherited(property, hideInChildrenAttribute))
				root.Add(CreatePropertyField(property));

			return root;
		}

		private bool IsPropertyInherited(SerializedProperty property, HideInChildrenAttribute attribute)
		{
			var targetObjectType = property.serializedObject.targetObject.GetType();
			var fieldInfo = ReflectionUtility.FindField(property.name, property);

			foreach (var type in attribute.ChildTypes)
			{
				if (targetObjectType != type)
					return false;
			}

			return targetObjectType != fieldInfo.DeclaringType;
		}
	}
}
