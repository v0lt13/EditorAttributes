using System;
using UnityEngine;
using EditorAttributes;

namespace EditorAttributesSamples
{
	[HelpURL("https://editorattributesdocs.readthedocs.io/en/latest/Attributes/GroupingAttributes/datatable.html")]
	public class DataTableSample : MonoBehaviour
	{
		[Serializable]
		private struct Data
		{
			public int intField;
			public float floatField;
			public string stringField;
			public bool boolField;
		}

		[Header("DataTable Attribute:")]
		[SerializeField, DataTable(true)] private Data structField;
		[SerializeField, DataTable] private Data[] structArray;
	}
}
