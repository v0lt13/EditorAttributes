using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;

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

		public const string PARAMS_DATA_LOCATION = "ProjectSettings/EditorAttributes";

		public static void DrawButton(MethodInfo function, ButtonAttribute buttonAttribute, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues, object target)
        {
			var prevGUIColor = GUI.color;
			var colorAttribute = function.GetCustomAttribute<GUIColorAttribute>() as IColorAttribute;
			colorAttribute ??= function.GetCustomAttribute<ColorFieldAttribute>();

			if (colorAttribute != null) ApplyButtonColor(colorAttribute);

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

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				if (GUILayout.Button(string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel, GUILayout.Height(buttonAttribute.ButtonHeight)))
					function.Invoke(target, parameterValues[function]);

				var foldoutStyle = new GUIStyle(EditorStyles.foldout)
				{
					margin = new RectOffset(17, 0, 0, 0),
					fontStyle = FontStyle.Bold
				};

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

			if (colorAttribute != null && colorAttribute.GetType() == typeof(ColorFieldAttribute)) GUI.color = prevGUIColor;
		}
		
		public static string GetFunctionID(MethodInfo function, object target) => $"{target}_{function.Name}_{string.Join("_", function.GetParameters().Select(param => param.ParameterType.Name))}";

		public static bool IsButtonFunction(MethodInfo function, out bool serializeParameters)
		{
			var buttonAttribute = function.GetCustomAttribute<ButtonAttribute>();

			if (buttonAttribute != null)
			{
				serializeParameters = buttonAttribute.SerializeParameters;

				return true;
			}

			serializeParameters = false;
			return false;
		}

		public static void SaveParamsData(MethodInfo[] functions, object target, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues)
		{
			var data = new FunctionParamData();
			var keyToMethod = new Dictionary<string, MethodInfo>();

			foreach (var function in functions)
			{
				if (!IsButtonFunction(function, out bool serializeParameters) || !serializeParameters) continue;

				string id = GetFunctionID(function, target);
				keyToMethod[id] = function;

				if (foldouts.TryGetValue(function, out bool foldoutValue)) data.foldouts[id] = foldoutValue;

				if (parameterValues.TryGetValue(function, out object[] parameterValue)) data.parameterValues[id] = parameterValue;
			}

			if (data.foldouts.Count == 0 && data.parameterValues.Count == 0) return; 

			string jsonData = JsonConvert.SerializeObject(data);
			File.WriteAllTextAsync(Path.Combine(PARAMS_DATA_LOCATION, $"{target}ParamsData.json"), jsonData);
		}

		public static void LoadParamsData(MethodInfo[] functions, object target, ref Dictionary<MethodInfo, bool> foldouts, ref Dictionary<MethodInfo, object[]> parameterValues)
		{
			if (!Directory.Exists(PARAMS_DATA_LOCATION)) Directory.CreateDirectory(PARAMS_DATA_LOCATION);

			try
			{
				var filePath = Path.Combine(PARAMS_DATA_LOCATION, $"{target}ParamsData.json");

				if (File.Exists(filePath))
				{
					string jsonData = File.ReadAllText(filePath);

					var data = JsonConvert.DeserializeObject<FunctionParamData>(jsonData);
					var keyToMethod = new Dictionary<string, MethodInfo>();

					foreach (var function in functions)
					{
						if (!IsButtonFunction(function, out bool serializeParameters) || !serializeParameters) continue;

						string id = GetFunctionID(function, target);

						keyToMethod[id] = function;
					}

					foreach (var key in data.foldouts.Keys)
					{
						if (keyToMethod.TryGetValue(key, out var method))
						{
							foldouts[method] = data.foldouts[key];
							parameterValues[method] = data.parameterValues[key];
						}
					}
				}
			}
			catch (ArgumentException)
			{
				return;
			}
		}
		
		public static void DeleteParamsData(string filePath)
		{
			if (File.Exists(filePath)) File.Delete(filePath);
		}

		private static void ApplyButtonColor(IColorAttribute colorAttribute)
		{
			if (ColorUtility.TryParseHtmlString(colorAttribute.HexColor, out Color color))
			{
				GUI.color = color;
			}
			else if (!string.IsNullOrEmpty(colorAttribute.HexColor))
			{
				EditorGUILayout.HelpBox($"The provided value {colorAttribute.HexColor} is not a valid Hex color", MessageType.Error);
			}
			else
			{
				GUI.color = PropertyDrawerBase.GUIColorToColor(colorAttribute);
			}
		}
	}
}
