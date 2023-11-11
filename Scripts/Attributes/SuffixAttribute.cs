using UnityEngine;

namespace EditorAttributes
{
    public class SuffixAttribute : PropertyAttribute
    {
	    public string Suffix { get; private set; }
		public float Offset { get; private set; }

		/// <summary>
		/// Attribute to add a suffix on a field
		/// </summary>
		/// <param name="suffix">The suffix to add</param>
		/// <param name="offset">Offset to add between the suffix and field</param>
		public SuffixAttribute(string suffix, float offset = 0f)
		{
			Suffix = suffix;
			Offset = offset;
		}
    }
}
