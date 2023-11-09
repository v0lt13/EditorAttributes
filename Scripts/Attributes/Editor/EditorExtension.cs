using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Object), true)]
	public class EditorExtension : UnityEditor.Editor
	{
		private Dictionary<MethodInfo, bool> foldouts = new();
		private Dictionary<MethodInfo, object[]> parameterValues = new();

		private MethodInfo[] functions;

		void OnEnable()
		{
			functions = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			//ButtonDrawer.LoadParamsData(functions, target, ref foldouts, ref parameterValues);
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			DrawButtons();

			//if (GUI.changed) ButtonDrawer.SaveParamsData(functions, target, foldouts, parameterValues);
		}

		private void DrawButtons()
		{
			foreach(var function in functions)
			{
				var buttonAttribute = function.GetCustomAttribute<ButtonAttribute>();

				if (buttonAttribute == null) continue;

				ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);
			}
		}
	}
}
