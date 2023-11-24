using UnityEngine;
using EditorAttributes;

namespace EditorAttributeSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/typefilter.html")]
	public class TypeFilterSample : MonoBehaviour, IFilter
	{
		[Header("TypeFilter Attribute:")]
		[SerializeField, TypeFilter(typeof(Transform))] private Component transformFilter;
		[SerializeField, TypeFilter(typeof(IFilter))] private Component interfaceFilter;
	}

	public interface IFilter { }
}
