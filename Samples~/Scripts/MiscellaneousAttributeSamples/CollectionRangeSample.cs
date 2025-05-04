using UnityEngine;
using EditorAttributes;
using System.Collections.Generic;

namespace EditorAttributesSamples
{
    [HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/collectionrange.html")]
    public class CollectionRangeSample : MonoBehaviour
    {
        [Header("CollectionRange Attribute:")]
        [SerializeField, CollectionRange(0, 5)] private int[] intArray;

        [SerializeField, CollectionRange(1, 7)] private List<string> stringList;
    }
}
