using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace EditorAttributes.Editor
{
    public class ButtonDrawer
    {
		[Serializable]
		private class FunctionParamData
		{
			public Dictionary<string, bool> foldouts = new();
			public Dictionary<string, object[]> parameterValues = new();
		}

		private const string PATH = "ProjectSettings/EditorAttributes";

		public static void DrawButton(MethodInfo function, ButtonAttribute buttonAttribute, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues, object target)
        {
			var functionParameters = function.GetParameters();

			if (functionParameters.Length > 0)
			{
				if (!parameterValues.ContainsKey(function))
				{
					parameterValues[function] = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						parameterValues[function][i] = functionParameters[i].DefaultValue;
				}

				if (!foldouts.ContainsKey(function)) foldouts[function] = true;

				EditorGUILayout.BeginVertical(new GUIStyle(EditorStyles.helpBox));

				if (GUILayout.Button(string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel, GUILayout.Height(buttonAttribute.ButtonHeight)))
					function.Invoke(target, parameterValues[function]);

				var foldoutStyle = new GUIStyle(EditorStyles.foldout) { margin = new RectOffset(17, 0, 0, 0) };

				foldouts[function] = EditorGUILayout.BeginFoldoutHeaderGroup(foldouts[function], "Parameters", foldoutStyle);

				if (foldouts[function])
				{
					for (int i = 0; i < functionParameters.Length; i++)
						parameterValues[function][i] = PropertyDrawerBase.DrawField(functionParameters[i].ParameterType, functionParameters[i].Name, parameterValues[function][i]);
				}

				EditorGUILayout.EndFoldoutHeaderGroup();
				EditorGUILayout.EndVertical();
			}
			else
			{
				if (GUILayout.Button(string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel, GUILayout.Height(buttonAttribute.ButtonHeight)))
					function.Invoke(target, null);
			}
		}
		
		public static string GetFunctionID(MethodInfo function, object target) => $"{target}_{function.Name}_{string.Join("_", function.GetParameters().Select(p => p.ParameterType.Name))}";

		public static bool IsButtonFunction(MethodInfo function)
		{
			var buttonAttribute = function.GetCustomAttribute<ButtonAttribute>();

			return buttonAttribute != null;
		}

		public static void SaveParamsData(MethodInfo[] functions, object target, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues)
		{
			var data = new FunctionParamData();
			var keyToMethod = new Dictionary<string, MethodInfo>();

			foreach (var function in functions)
			{
				if (!IsButtonFunction(function)) continue;

				string id = GetFunctionID(function, target);
				keyToMethod[id] = function;

				if (foldouts.TryGetValue(function, out bool foldoutValue)) data.foldouts[id] = foldoutValue;

				if (parameterValues.TryGetValue(function, out object[] parameterValue)) data.parameterValues[id] = parameterValue;
			}

			string jsonData = JsonConvert.SerializeObject(data);
			File.WriteAllTextAsync(Path.Combine(PATH, $"{target}ParamsData.json"), jsonData);
		}

		public static void LoadParamsData(MethodInfo[] functions, object target, ref Dictionary<MethodInfo, bool> foldouts, ref Dictionary<MethodInfo, object[]> parameterValues)
		{
			if (!Directory.Exists(PATH)) Directory.CreateDirectory(PATH);

			var filePath = Path.Combine(PATH, $"{target}ParamsData.json");

			if (File.Exists(filePath))
			{
				string jsonData = File.ReadAllText(filePath);

				var data = JsonConvert.DeserializeObject<FunctionParamData>(jsonData);
				var keyToMethod = new Dictionary<string, MethodInfo>();

				foreach (var function in functions)
				{
					if (!IsButtonFunction(function)) continue;

					string id = GetFunctionID(function, target);

					keyToMethod[id] = function;
				}

				var newFoldouts = new Dictionary<MethodInfo, bool>();
				var newParameterValues = new Dictionary<MethodInfo, object[]>();

				foreach (var key in data.foldouts.Keys)
				{
					if (keyToMethod.TryGetValue(key, out var method))
					{
						foldouts[method] = data.foldouts[key];
						parameterValues[method] = data.parameterValues[key];
					}
				}

				foldouts = newFoldouts;
				parameterValues = newParameterValues;
			}
		}		
	}
}
