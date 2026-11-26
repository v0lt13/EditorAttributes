using UnityEngine;

namespace EditorAttributes.Attributes
{
    public abstract class AnimationDropdownAttribute : PropertyAttribute
    {
        public string FieldName { get; }

        protected AnimationDropdownAttribute(string fieldName) => FieldName = fieldName;
    }

    /// <summary>
    /// Attribute to display a dropdown of animator state names (valid targets for Animator.Play)
    /// </summary>
    public class AnimationNameDropdownAttribute : AnimationDropdownAttribute
    {
        /// <summary>
        /// Attribute to display a dropdown of animator state names (valid targets for Animator.Play)
        /// </summary>
        /// <param name="fieldName">The Animator or RuntimeAnimatorController field to pull the state names from</param>
        public AnimationNameDropdownAttribute(string fieldName = "") : base(fieldName) { }
    }

    /// <summary>
    /// Attribute to display a dropdown of animator parameters
    /// </summary>
    public class AnimationParamDropdownAttribute : AnimationDropdownAttribute
    {
        /// <summary>
        /// Attribute to display a dropdown of animator parameters
        /// </summary>
        /// <param name="fieldName">The Animator or RuntimeAnimatorController field to pull the parameters from</param>
        public AnimationParamDropdownAttribute(string fieldName = "") : base(fieldName) { }
    }
}