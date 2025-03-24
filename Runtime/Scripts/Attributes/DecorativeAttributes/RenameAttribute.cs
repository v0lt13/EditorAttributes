using UnityEngine;

namespace EditorAttributes
{
	public enum CaseType
    {
        None,
        Unity,
        Pascal,
        Camel,
        Snake,
        Kebab,
        Upper,
        Lower
    }

	/// <summary>
	/// Attribute to rename a field in the inspector
	/// </summary>
	public class RenameAttribute : PropertyAttribute, IDynamicStringAttribute
    {
	    public string Name { get; private set; }
	    public CaseType CaseType { get; private set; }
		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to rename a field in the inspector
		/// </summary>
		/// <param name="name">The new name of the field</param>
		/// <param name="caseType">In what case to rename the field</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		public RenameAttribute(string name, CaseType caseType = CaseType.Unity, StringInputMode stringInputMode = StringInputMode.Constant)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
		{
			Name = name;
            CaseType = caseType;
            StringInputMode = stringInputMode;
        }
    }
}
