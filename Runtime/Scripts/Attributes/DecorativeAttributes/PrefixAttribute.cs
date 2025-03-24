using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// Attribute to add a prefix on a field
	/// </summary>
	public class PrefixAttribute : PropertyAttribute, IDynamicStringAttribute
    {
	    public string Prefix { get; private set; }
		public float Offset { get; private set; }

		public StringInputMode StringInputMode { get; private set; }

		/// <summary>
		/// Attribute to add a prefix on a field
		/// </summary>
		/// <param name="prefix">The prefix to add</param>
		/// <param name="offset">Offset to add between the prefix and field in pixels</param>
		/// <param name="stringInputMode">Set if the string input is set trough a constant or dynamically trough another member</param>
		public PrefixAttribute(string prefix, float offset = 10f, StringInputMode stringInputMode = StringInputMode.Constant)
		{
			Prefix = prefix;
			Offset = offset;
			StringInputMode = stringInputMode;
		}
	}
}
