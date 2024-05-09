using UnityEngine;

namespace EditorAttributes
{
    public class AnimatorParamDropdownAttribute : PropertyAttribute
    {
		public string AnimatorFieldName { get; private set; }

		/// <summary>
		/// Attribute to display a dropdown of animator parameters
		/// </summary>
		/// <param name="animatorFieldName">The animator from which to get the parameters</param>
		public AnimatorParamDropdownAttribute(string animatorFieldName) => AnimatorFieldName = animatorFieldName;
	}
}
