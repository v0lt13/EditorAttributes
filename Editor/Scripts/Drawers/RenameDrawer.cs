using UnityEngine;
using UnityEditor;
using System.Text;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(RenameAttribute))]
    public class RenameDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
			var renameAttribute = attribute as RenameAttribute;
            var newName = GetDynamicString(renameAttribute.Name, property, renameAttribute);

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

            DrawProperty(position, property, new GUIContent(newName));
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
