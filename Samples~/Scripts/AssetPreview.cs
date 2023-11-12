using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
    public class AssetPreview : MonoBehaviour
    {
		[SerializeField, AssetPreview] private Sprite spriteAsset;
		[SerializeField, AssetPreview(64f, 64f)] private Material materialAsset;
		[SerializeField, AssetPreview(32f, 32f)] private Mesh meshAsset;
	}
}
