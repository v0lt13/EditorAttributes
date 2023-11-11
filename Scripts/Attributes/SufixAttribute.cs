using UnityEngine;

namespace EditorAttributes
{
    public class SufixAttribute : PropertyAttribute
    {
	    public string Sufix { get; private set; }
		public float Offset { get; private set; }

		/// <summary>
		/// Attribute to add a sufix on a field
		/// </summary>
		/// <param name="sufix">The sufix to add</param>
		/// <param name="offset">Offset to add between the sufix and field</param>
		public SufixAttribute(string sufix, float offset = 0f)
		{
			Sufix = sufix;
			Offset = offset;
		}
    }
}
