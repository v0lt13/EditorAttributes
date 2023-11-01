using UnityEngine;

namespace EditorAttributes
{
    public enum CaseType
    {
        Pascal,
        Camel,
        Snake,
        Kebab,
        Upper,
        Lower
    }

    public class RenameAttribute : PropertyAttribute
    {
	    public string Name { get; set; }
	    public CaseType CaseType { get; set; }

        /// <summary>
        /// Rename a field in the inspector
        /// </summary>
        /// <param name="name">The new name of the field</param>
        /// <param name="caseType">In what case to rename the field</param>
        public RenameAttribute(string name, CaseType caseType = CaseType.Pascal)
        {
            Name = name;
            CaseType = caseType;
        }
    }
}
