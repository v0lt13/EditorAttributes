using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/typefilter.html")]
	public class TypeFilterSample : MonoBehaviour, IFilter
	{
		[Header("TypeFilter Attribute:")]
		[SerializeField, TypeFilter(typeof(BoxCollider), typeof(SphereCollider))] private Component colliderFilter;
		[SerializeField, TypeFilter(typeof(IFilter))] private Component interfaceFilter;
	}

	public interface IFilter { }
}
