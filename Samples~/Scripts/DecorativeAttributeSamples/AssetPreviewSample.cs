using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/DecorativeAttributes/assetpreview.html")]
	public class AssetPreviewSample : MonoBehaviour
    {
		[Header("AssetPreview attribute:")]
		[SerializeField, AssetPreview] private Sprite spriteAsset;
		[SerializeField, AssetPreview(64f, 64f)] private Material materialAsset;
		[SerializeField, AssetPreview(32f, 32f)] private Mesh meshAsset;
	}
}
