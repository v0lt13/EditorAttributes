using UnityEngine;

namespace EditorAttributes
{
    public class SuffixAttribute : PropertyAttribute, IDynamicStringAttribute
    {
	    public string Suffix { get; private set; }
		public float Offset { get; private set; }
		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to add a suffix on a field
		/// </summary>
		/// <param name="suffix">The suffix to add</param>
		/// <param name="offset">Offset to add between the suffix and field in pixels</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		public SuffixAttribute(string suffix, float offset = 10f, StringInputMode stringInputMode = StringInputMode.Constant)
		{
			Suffix = suffix;
			Offset = offset;
			StringInputMode = stringInputMode;
		}
	}
}
