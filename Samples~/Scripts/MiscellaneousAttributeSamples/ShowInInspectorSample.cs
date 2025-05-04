using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
    [HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/showininspector.html")]
    public class ShowInInspectorSample : MonoBehaviour
    {
        [Header("ShowInInspector Attribute:")]
        [SerializeField] private Void headerHolder;

        [ShowInInspector] public static int field = 25;

        [ShowInInspector] public string Property { get; set; } = "This is a non serialized property";

        [ShowInInspector] public bool Method() => true;
    }
}
