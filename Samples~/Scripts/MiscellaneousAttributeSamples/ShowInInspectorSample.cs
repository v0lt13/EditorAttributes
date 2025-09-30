using System;
using UnityEngine;
using EditorAttributes;
using Void = EditorAttributes.Void;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/MiscellaneousAttributes/showininspector.html")]
	public class ShowInInspectorSample : MonoBehaviour
	{
		[Serializable]
		public class Data
		{
			public long longField = 1243769872163872163;
			public float floatField = 5.5f;
			public char charField = 'w';
		}

		[Header("ShowInInspector Attribute:")]
		[SerializeField] private Void headerHolder;

		[ShowInInspector] public static int field = 25;

		[ShowInInspector] private Data customObject;

		[ShowInInspector] public string Property { get; set; } = "This is a non serialized property";

		[ShowInInspector] public bool Method() => true;
	}
}
