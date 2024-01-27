using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/filepath.html")]
	public class FilePathSample : MonoBehaviour
	{
		[Header("FilePath Attribute:")]
		[SerializeField, FilePath] private string filePath;
		[SerializeField, FilePath(false)] private string absoluteFilePath;
		[SerializeField, FilePath(filters:"cs,unity")] private string filteredFilePath;
	}
}
