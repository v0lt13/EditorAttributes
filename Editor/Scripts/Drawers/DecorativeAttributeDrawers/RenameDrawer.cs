using UnityEditor;
using System.Text;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();
			var errorBox = new HelpBox();
			var propertyField = DrawProperty(property, new Label(property.displayName));

			UpdateVisualElement(root, () =>
			{
				propertyField.Q<Label>().text = GetNewName(property, errorBox);
				DisplayErrorBox(root, errorBox);
			});

			root.Add(propertyField);

			return root;
		}

        private string GetNewName(SerializedProperty property, HelpBox errorBox)
        {
			var renameAttribute = attribute as RenameAttribute;
			var newName = GetDynamicString(renameAttribute.Name, property, renameAttribute, errorBox);

			switch (renameAttribute.CaseType)
			{
				case CaseType.Unity:
					newName = ObjectNames.NicifyVariableName(newName);
					break;

				case CaseType.Pascal:
					var pascalName = char.ToUpper(newName[0]) + newName[1..];

					FormatString(ref pascalName);

					newName = pascalName;
					break;

				case CaseType.Camel:
					var camelName = char.ToLower(newName[0]) + newName[1..];

					FormatString(ref camelName);

					newName = camelName;
					break;

				case CaseType.Snake:
					newName = newName.Replace(" ", "_");
					break;

				case CaseType.Kebab:
					newName = newName.Replace(" ", "-");
					break;

				case CaseType.Upper:
					newName = newName.ToUpper();
					break;

				case CaseType.Lower:
					newName = newName.ToLower();
					break;
			}

			return newName;
		}

        private void FormatString(ref string stringToFormat)
        {
			while (stringToFormat.Contains(" "))
			{
				var spaceIndex = stringToFormat.IndexOf(" ");
                var charAfterSpace = stringToFormat[spaceIndex + 1];
                var stringBuilder = new StringBuilder(stringToFormat);

                stringBuilder.Replace(charAfterSpace, char.ToUpper(charAfterSpace), spaceIndex + 1, 1);

				stringToFormat = stringBuilder.ToString();
				stringToFormat = stringToFormat.Remove(spaceIndex, 1);
			}
		}
    }
}
