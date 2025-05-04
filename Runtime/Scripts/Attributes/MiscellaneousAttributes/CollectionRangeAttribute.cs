using UnityEngine;

namespace EditorAttributes
{
    /// <summary>
    /// Attribute to limit the size of a collection between a range
    /// </summary>
    public class CollectionRangeAttribute : PropertyAttribute
    {
        public int MinRange { get; private set; }
        public int MaxRange { get; private set; }

        /// <summary>
        /// Attribute to limit the size of a collection between a range
        /// </summary>
        /// <param name="minRange">Minimum size of the collection</param>
        /// <param name="maxRange">Maximum size of the collection</param>
        public CollectionRangeAttribute(int minRange, int maxRange)
#if UNITY_2023_3_OR_NEWER
        : base(true) 
#endif
        {
            MinRange = minRange;
            MaxRange = maxRange;
        }
    }
}
