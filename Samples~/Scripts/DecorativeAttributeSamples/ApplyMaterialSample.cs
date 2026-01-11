using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
    [HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/applymaterial.html")]
    public class ApplyMaterialSample : MonoBehaviour
    {
        [Header("ApplyMaterial Attribute:")]
        [SerializeField, ApplyMaterial(nameof(material))] private int intField;

        [SerializeField] private Material material;
    }
}
