using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
using Object = UnityEngine.Object;

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

		internal const string PARAMS_DATA_LOCATION = "ProjectSettings/EditorAttributes";

		internal static VisualElement DrawButton(MethodInfo function, ButtonAttribute buttonAttribute, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues, Object[] targets)
		{
			var root = new VisualElement();

			var functionParameters = function.GetParameters();

			if (functionParameters.Length > 0)
			{
				// Parameter default setup
				if (!parameterValues.ContainsKey(function))
				{
					parameterValues[function] = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						parameterValues[function][i] = functionParameters[i].DefaultValue;
				}

				if (!foldouts.ContainsKey(function))
					foldouts[function] = true;

				// Create the button
				var button = MakeButton(function, buttonAttribute, () =>
				{
					var paramValueList = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						paramValueList[i] = ConvertParameterValue(functionParameters[i].ParameterType, parameterValues[function][i]);

					InvokeFunctionOnAllTargets(targets, function.Name, parameterValues[function]);
				});

				// Styling
				var foldout = new Foldout
				{
					text = "Parameters",
					value = foldouts[function]
				};

				PropertyDrawerBase.ApplyBoxStyle(root);
				PropertyDrawerBase.ApplyBoxStyle(foldout);

				foldout.style.unityFontStyleAndWeight = FontStyle.Bold;
				foldout.style.paddingLeft = 15f;

				if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
				{
					button.style.color = EditorExtension.GLOBAL_COLOR;
					foldout.style.color = EditorExtension.GLOBAL_COLOR;
				}

				foldout.RegisterValueChangedCallback((callback) => foldouts[function] = callback.newValue);

				// Create parameter fields
				for (int i = 0; i < functionParameters.Length; i++)
				{
					var parameter = functionParameters[i];

					if (!parameter.ParameterType.IsPrimitive && parameter.ParameterType != typeof(string))
					{
						foldout.Add(new HelpBox($"Parameter type {parameter.ParameterType} is not supported. Only Unity supported primitive types (and strings) are supported.", HelpBoxMessageType.Error));
						continue;
					}

					var field = PropertyDrawerBase.CreateFieldForType(parameter.ParameterType, parameter.Name, ConvertParameterValue(parameter.ParameterType, parameterValues[function][i]));

					if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
						ColorUtils.ApplyColor(field, EditorExtension.GLOBAL_COLOR);

					int index = i; // Local copy for the lambda

					PropertyDrawerBase.RegisterValueChangedCallbackByType(parameter.ParameterType, field, (valueCallback) => parameterValues[function][index] = valueCallback);

					field.SetEnabled(targets.Length <= 1);
					field.style.unityFontStyleAndWeight = FontStyle.Normal;

					foldout.Add(field);
				}

				root.Add(button);
				root.Add(foldout);
			}
			else
			{
				var button = MakeButton(function, buttonAttribute, () => InvokeFunctionOnAllTargets(targets, function.Name, null));

				if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
					button.style.color = EditorExtension.GLOBAL_COLOR;

				root.Add(button);
			}

			return root;
		}

		private static VisualElement MakeButton(MethodInfo function, ButtonAttribute buttonAttribute, Action buttonLogic)
		{
			var buttonLabel = string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel;
			var tooltipAttribute = function?.GetCustomAttribute<TooltipAttribute>();
			string buttonTooltip = string.Empty;

			if (tooltipAttribute != null)
				buttonTooltip = tooltipAttribute.tooltip;

			if (buttonAttribute.IsRepetable)
			{
				var repeatButton = new RepeatButton(buttonLogic, buttonAttribute.PressDelay, buttonAttribute.RepetitionInterval)
				{
					text = buttonLabel,
					tooltip = buttonTooltip
				};

				repeatButton.style.height = buttonAttribute.ButtonHeight;
				repeatButton.AddToClassList(Button.ussClassName);

				return repeatButton;
			}
			else
			{
				var button = new Button(buttonLogic)
				{
					text = buttonLabel,
					tooltip = buttonTooltip
				};

				button.style.height = buttonAttribute.ButtonHeight;

				return button;
			}
		}

		private static void InvokeFunctionOnAllTargets(Object[] targets, string functionName, object[] parameterValues)
		{
			foreach (var target in targets)
			{
				var methodInfo = ReflectionUtility.FindFunction(functionName, target);
				var functionParameters = methodInfo.GetParameters();

				object[] paramValueList = null;

				if (functionParameters.Length > 0)
				{
					paramValueList = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						paramValueList[i] = ConvertParameterValue(functionParameters[i].ParameterType, parameterValues[i]);
				}

				Undo.RecordObject(target, $"Invoke {functionName}");

				methodInfo.Invoke(target, paramValueList);

				EditorUtility.SetDirty(target);
			}
		}

		internal static void SaveParamsData(MethodInfo[] functions, object target, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues)
		{
			var data = new FunctionParamData();
			var keyToMethod = new Dictionary<string, MethodInfo>();

			foreach (var function in functions)
			{
				if (!IsButtonFunction(function, out bool serializeParameters) || !serializeParameters)
					continue;

				string id = GetFunctionID(function, target);
				keyToMethod[id] = function;

				if (foldouts.TryGetValue(function, out bool foldoutValue))
					data.foldouts[id] = foldoutValue;

				if (parameterValues.TryGetValue(function, out object[] parameterValue))
					data.parameterValues[id] = parameterValue;
			}

			if (data.foldouts.Count == 0 && data.parameterValues.Count == 0)
				return;

			JsonConvert.DefaultSettings = () => new JsonSerializerSettings { Converters = { new UnityTypeConverter() } };

			string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

			File.WriteAllTextAsync(Path.Combine(PARAMS_DATA_LOCATION, GetFileName(target)), jsonData);
		}

		internal static void LoadParamsData(MethodInfo[] functions, object target, ref Dictionary<MethodInfo, bool> foldouts, ref Dictionary<MethodInfo, object[]> parameterValues)
		{
			if (!Directory.Exists(PARAMS_DATA_LOCATION))
				Directory.CreateDirectory(PARAMS_DATA_LOCATION);

			try
			{
				var filePath = Path.Combine(PARAMS_DATA_LOCATION, GetFileName(target));

				if (File.Exists(filePath))
				{
					string jsonData = File.ReadAllText(filePath);

					var data = JsonConvert.DeserializeObject<FunctionParamData>(jsonData);
					var keyToMethod = new Dictionary<string, MethodInfo>();

					foreach (var function in functions)
					{
						if (!IsButtonFunction(function, out bool serializeParameters) || !serializeParameters)
							continue;

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

		internal static void DeleteParamsData(string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);
		}

		internal static void ClearAllParamsData()
		{
			if (Directory.Exists(PARAMS_DATA_LOCATION))
			{
				int fileCount = 0;

				foreach (var file in Directory.GetFiles(PARAMS_DATA_LOCATION, "*_ButtonParameterData.json"))
				{
					File.Delete(file);
					fileCount++;
				}

				Debug.Log($"<b>{fileCount}</b> files were deleted");
			}
		}

		internal static string GetFileName(object target) => $"{(target as Object).GetInstanceID()}_{target}_ButtonParameterData.json";

		internal static string GetFunctionID(MethodInfo function, object target) => $"{(target as Object).GetInstanceID()}_{target}_{function.Name}_{string.Join("_", function.GetParameters().Select(param => param.ParameterType.Name))}";

		internal static bool IsButtonFunction(MethodInfo function, out bool serializeParameters)
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

		private static T ParseFromJson<T>(object value)
		{
			if (value == null)
				return default;

			try // Try to see if we can cast the value directly
			{
				return (T)value;
			}
			catch (InvalidCastException)
			{
				string jsonString = value.ToString();

				try
				{
					return string.IsNullOrEmpty(jsonString) ? default : JsonConvert.DeserializeObject<T>(jsonString);
				}
				catch (JsonReaderException)
				{
					return default;
				}
			}
		}

		private static object ConvertParameterValue(Type parameterType, object parameterValue)
		{
			bool isDBNull = Convert.IsDBNull(parameterValue);

			if (parameterType == typeof(string))
			{
				return parameterValue?.ToString();
			}
			else if (parameterType == typeof(char))
			{
				string stringParamValue = parameterValue.ToString();

				return isDBNull || stringParamValue.Length != 1 ? '\0' : Convert.ToChar(parameterValue);
			}
			else if (parameterType == typeof(int))
			{
				return isDBNull ? 0 : Convert.ToInt32(parameterValue);
			}
			else if (parameterType == typeof(uint))
			{
				return isDBNull ? 0 : Convert.ToUInt32(parameterValue);
			}
			else if (parameterType == typeof(long))
			{
				return isDBNull ? 0 : Convert.ToInt64(parameterValue);
			}
			else if (parameterType == typeof(ulong))
			{
				return isDBNull ? 0 : Convert.ToUInt64(parameterValue);
			}
			else if (parameterType == typeof(float))
			{
				return isDBNull ? 0.0f : Convert.ToSingle(parameterValue);
			}
			else if (parameterType == typeof(double))
			{
				return isDBNull ? 0.0 : (double)parameterValue;
			}
			else if (parameterType == typeof(bool))
			{
				return !isDBNull && (bool)parameterValue;
			}
			else if (parameterType.IsEnum)
			{
				return isDBNull ? Enum.ToObject(parameterType, 0) as Enum : Enum.ToObject(parameterType, parameterValue) as Enum;
			}
			else if (parameterType == typeof(Vector2))
			{
				return isDBNull ? Vector2.zero : ParseFromJson<Vector2>(parameterValue);
			}
			else if (parameterType == typeof(Vector2Int))
			{
				return isDBNull ? Vector2Int.zero : ParseFromJson<Vector2Int>(parameterValue);
			}
			else if (parameterType == typeof(Vector3))
			{
				return isDBNull ? Vector3.zero : ParseFromJson<Vector3>(parameterValue);
			}
			else if (parameterType == typeof(Vector3Int))
			{
				return isDBNull ? Vector3Int.zero : ParseFromJson<Vector3Int>(parameterValue);
			}
			else if (parameterType == typeof(Vector4))
			{
				return isDBNull ? Vector4.zero : ParseFromJson<Vector4>(parameterValue);
			}
			else if (parameterType == typeof(Color))
			{
				return isDBNull ? Color.black : ParseFromJson<Color>(parameterValue);
			}
			else if (parameterType == typeof(Gradient))
			{
				return isDBNull ? new Gradient() : ParseFromJson<Gradient>(parameterValue);
			}
			else if (parameterType == typeof(AnimationCurve))
			{
				return isDBNull ? AnimationCurve.Linear(0f, 0f, 1f, 1f) : ParseFromJson<AnimationCurve>(parameterValue);
			}
			else if (parameterType == typeof(LayerMask))
			{
				return isDBNull ? (LayerMask)0 : (LayerMask)Convert.ToInt32(parameterValue);
			}
			else if (parameterType == typeof(Rect))
			{
				return isDBNull ? new Rect(0f, 0f, 0f, 0f) : ParseFromJson<Rect>(parameterValue);
			}
			else if (parameterType == typeof(RectInt))
			{
				return isDBNull ? new RectInt(0, 0, 0, 0) : ParseFromJson<RectInt>(parameterValue);
			}
			else if (parameterType == typeof(Bounds))
			{
				return isDBNull ? new Bounds(new(0, 0), new(0, 0)) : ParseFromJson<Bounds>(parameterValue);
			}
			else if (parameterType == typeof(BoundsInt))
			{
				return isDBNull ? new BoundsInt(new(0, 0), new(0, 0)) : ParseFromJson<BoundsInt>(parameterValue);
			}

			return null;
		}
	}
}
