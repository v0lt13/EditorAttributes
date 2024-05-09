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
			var root = new VisualElement();

			if (!IsPropertyInherited(property))
				root.Add(DrawProperty(property));

			return root;
		}

		private bool IsPropertyInherited(SerializedProperty property)
		{
			var targetObjectType = property.serializedObject.targetObject.GetType();
			var fieldInfo = targetObjectType.GetField(property.name, ReflectionUtility.BINDING_FLAGS);

			return fieldInfo == null; // If fieldInfo is null it means that the field was not found in the target, so it must be inherited
		}
	}
}
