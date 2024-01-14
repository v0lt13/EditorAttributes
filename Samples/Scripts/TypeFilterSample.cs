using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/typefilter.html")]
	public class TypeFilterSample : MonoBehaviour, IFilter
	{
		[Header("TypeFilter Attribute:")]
		[SerializeField, TypeFilter(typeof(Transform))] private Component transformFilter;
		[SerializeField, TypeFilter(typeof(IFilter))] private Component interfaceFilter;
	}

	public interface IFilter { }
}
