using UnityEngine;

namespace EditorAttributes
{
    public enum CaseType
    {
        None,
        Pascal,
        Camel,
        Snake,
        Kebab,
        Upper,
        Lower
    }

    public class RenameAttribute : PropertyAttribute
    {
	    public string Name { get; private set; }
	    public CaseType CaseType { get; private set; }

        /// <summary>
        /// Attribute to rename a field in the inspector
        /// </summary>
        /// <param name="name">The new name of the field</param>
        /// <param name="caseType">In what case to rename the field</param>
        public RenameAttribute(string name, CaseType caseType = CaseType.None)
        {
            Name = name;
            CaseType = caseType;
        }
    }
}
