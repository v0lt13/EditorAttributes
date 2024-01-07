using UnityEngine;

namespace EditorAttributes
{
    public class PrefixAttribute : PropertyAttribute
    {
	    public string Prefix { get; private set; }
		public float Offset { get; private set; }

		/// <summary>
		/// Attribute to add a prefix on a field
		/// </summary>
		/// <param name="prefix">The prefix to add</param>
		/// <param name="offset">Offset to add between the prefix and field</param>
		public PrefixAttribute(string prefix, float offset = 0f)
		{
			Prefix = prefix;
			Offset = offset;
		}
	}
}
