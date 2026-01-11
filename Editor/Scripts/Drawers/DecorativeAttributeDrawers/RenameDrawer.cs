using UnityEditor;
using System.Text;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var renameAttribute = attribute as RenameAttribute;

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property, GetNewName(renameAttribute, property, errorBox));

            if (renameAttribute.StringInputMode == StringInputMode.Dynamic)
            {
                propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
                {
                    var propertyLabel = propertyField.Q<Label>();

                    UpdateVisualElement(propertyField, () =>
                    {
                        if (propertyLabel != null)
                            propertyLabel.text = GetNewName(renameAttribute, property, errorBox);
                    });
                });

                DisplayErrorBox(propertyField, errorBox);
            }

            return propertyField;
        }

        internal static string GetNewName(RenameAttribute renameAttribute, SerializedProperty property, HelpBox errorBox)
        {
            string newName = GetDynamicString(renameAttribute.Name, property, renameAttribute, errorBox);

            switch (renameAttribute.CaseType)
            {
                case CaseType.None:
                    return newName;

                case CaseType.Unity:
                    newName = ObjectNames.NicifyVariableName(newName);
                    break;

                case CaseType.Pascal:
                    string pascalName = char.ToUpper(newName[0]) + newName[1..];

                    FormatString(ref pascalName);

                    newName = pascalName;
                    break;

                case CaseType.Camel:
                    string camelName = char.ToLower(newName[0]) + newName[1..];

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

        private static void FormatString(ref string stringToFormat)
        {
            while (stringToFormat.Contains(" "))
            {
                int spaceIndex = stringToFormat.IndexOf(" ");
                char charAfterSpace = stringToFormat[spaceIndex + 1];
                StringBuilder stringBuilder = new(stringToFormat);

                stringBuilder.Replace(charAfterSpace, char.ToUpper(charAfterSpace), spaceIndex + 1, 1);

                stringToFormat = stringBuilder.ToString();
                stringToFormat = stringToFormat.Remove(spaceIndex, 1);
            }
        }
    }
}
