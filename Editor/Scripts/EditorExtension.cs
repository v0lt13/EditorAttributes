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
			functions = target.GetType().GetMethods(ReflectionUtility.BINDING_FLAGS);

			ButtonDrawer.LoadParamsData(functions, target, ref foldouts, ref parameterValues);

			try
			{
				buttonParamsDataFilePath = Path.Combine(ButtonDrawer.PARAMS_DATA_LOCATION, $"{target}ParamsData.json");
			}
			catch (System.ArgumentException)
			{
				return;
			}
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

				var conditionalProperty = ReflectionUtility.GetValidMemberInfo(buttonAttribute.ConditionName, target);

				if (conditionalProperty != null)
				{
					var conditionValue = PropertyDrawerBase.GetConditionValue(conditionalProperty, buttonAttribute, target);

					if (buttonAttribute.Negate) conditionValue = !conditionValue;

					switch (buttonAttribute.ConditionResult)
					{
						case ConditionResult.ShowHide:
							if (conditionValue) ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);
							break;

						case ConditionResult.EnableDisable:
							using (var group = new EditorGUI.DisabledGroupScope(!conditionValue))
								ButtonDrawer.DrawButton(function, buttonAttribute, foldouts, parameterValues, target);

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
