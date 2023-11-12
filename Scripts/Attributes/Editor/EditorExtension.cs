using System.IO;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
	[CanEditMultipleObjects, CustomEditor(typeof(Object), true)]
	public class EditorExtension : UnityEditor.Editor
	{
		private string buttonParamsDataFilePath;

		private Dictionary<MethodInfo, bool> foldouts = new();
		private Dictionary<MethodInfo, object[]> parameterValues = new();

		private MethodInfo[] functions;

		void OnEnable()
		{
			functions = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			ButtonDrawer.LoadParamsData(functions, target, ref foldouts, ref parameterValues);

			buttonParamsDataFilePath = Path.Combine(ButtonDrawer.PARAMS_DATA_LOCATION, $"{target}ParamsData.json");
		}

		void OnDisable()
		{
			if (target == null) ButtonDrawer.DeleteParamsData(buttonParamsDataFilePath);
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			DrawButtons();

			if (GUI.changed) ButtonDrawer.SaveParamsData(functions, target, foldouts, parameterValues);
		}

		private void DrawButtons()
		{
			foreach(var function in functions)
			{
				var buttonAttribute = function.GetCustomAttribute<ButtonAttribute>();

				if (buttonAttribute == null) continue;

				var conditionalProperty = PropertyDrawerBase.GetValidMemberInfo(buttonAttribute.ConditionName, target);

				if (conditionalProperty != null)
				{
					var conditionValue = PropertyDrawerBase.GetConditionValue<ButtonAttribute>(conditionalProperty, buttonAttribute, target, true);

					if (buttonAttribute.Negate) conditionValue = !conditionValue;

					switch (buttonAttribute.ConditionResult)
					{
						case ConditionResult.ShowHide:
							if (conditionValue) ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);
							break;

						case ConditionResult.EnableDisable:
							GUI.enabled = conditionValue;

							ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);

							GUI.enabled = true;
							break;
					}
				}
				else
				{
					ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);
				}
			}
		}
	}
}
