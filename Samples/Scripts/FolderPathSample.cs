using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/folderpath.html")]
	public class FolderPathSample : MonoBehaviour
	{
		[Header("FolderPath Attribute:")]
		[SerializeField, FolderPath] private string folderPath;
		[SerializeField, FolderPath(false)] private string absoluteFolderPath;
	}
}
