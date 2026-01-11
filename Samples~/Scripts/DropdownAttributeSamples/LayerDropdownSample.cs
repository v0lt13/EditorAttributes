using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
    [HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DropdownAttributes/layerdropdown.html")]
    public class LayerDropdownSample : MonoBehaviour
    {
        [Header("LayerDropdown Attribute:")]
        [SerializeField, LayerDropdown] private string layer;
    }
}
