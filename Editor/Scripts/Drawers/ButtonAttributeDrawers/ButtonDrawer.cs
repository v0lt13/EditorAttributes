using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

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

		internal static VisualElement DrawButton(MethodInfo function, ButtonAttribute buttonAttribute, Dictionary<MethodInfo, bool> foldouts, Dictionary<MethodInfo, object[]> parameterValues, object target)
		{
			var root = new VisualElement();

			var functionParameters = function.GetParameters();

			if (functionParameters.Length > 0)
			{
				PropertyDrawerBase.ApplyBoxStyle(root);

				if (!parameterValues.ContainsKey(function))
				{
					parameterValues[function] = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						parameterValues[function][i] = functionParameters[i].DefaultValue;
				}

				if (!foldouts.ContainsKey(function))
					foldouts[function] = true;

				var button = MakeButton(function, buttonAttribute, () =>
				{
					var paramValueList = new object[functionParameters.Length];

					for (int i = 0; i < functionParameters.Length; i++)
						paramValueList[i] = ConvertParameterValue(functionParameters[i].ParameterType, parameterValues[function][i]);

					function.Invoke(target, paramValueList);
				});

				var foldout = new Foldout
				{
					text = "Parameters",
					value = foldouts[function]
				};

				PropertyDrawerBase.ApplyBoxStyle(foldout);

				foldout.style.unityFontStyleAndWeight = FontStyle.Bold;
				foldout.style.paddingLeft = 15f;

				if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
				{
					button.style.color = EditorExtension.GLOBAL_COLOR;
					foldout.style.color = EditorExtension.GLOBAL_COLOR;
				}

				foldout.RegisterValueChangedCallback((callback) => foldouts[function] = callback.newValue);

				for (int i = 0; i < functionParameters.Length; i++)
				{
					var parameter = functionParameters[i];
					var field = DrawParameterField(parameter.ParameterType, parameter.Name, parameterValues[function][i]);

					if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
						ColorUtils.ApplyColor(field, EditorExtension.GLOBAL_COLOR);

					int index = i;
					RegisterParameterFieldValueChangedCallback(field, parameter.ParameterType, (valueCallback) => parameterValues[function][index] = valueCallback);

					field.style.unityFontStyleAndWeight = FontStyle.Normal;
					foldout.Add(field);
				}

				root.Add(button);
				root.Add(foldout);
			}
			else
			{
				var button = MakeButton(function, buttonAttribute, () => function.Invoke(target, null));

				root.Add(button);
			}

			return root;
		}

		private static VisualElement MakeButton(MethodInfo function, ButtonAttribute buttonAttribute, Action buttonLogic)
		{
			var buttonLabel = string.IsNullOrWhiteSpace(buttonAttribute.ButtonLabel) ? function.Name : buttonAttribute.ButtonLabel;

			if (buttonAttribute.IsRepetable)
			{
				var repeatButton = new RepeatButton(buttonLogic, buttonAttribute.PressDelay, buttonAttribute.RepetitionInterval) { text = buttonLabel };

				repeatButton.style.height = buttonAttribute.ButtonHeight;
				repeatButton.AddToClassList("unity-button");

				return repeatButton;
			}
			else
			{
				var button = new Button(buttonLogic) { text = buttonLabel };

				button.style.height = buttonAttribute.ButtonHeight;

				return button;
			}
		}

		#region SERIALIZATION
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
			File.WriteAllTextAsync(Path.Combine(PARAMS_DATA_LOCATION, $"{target}ParamsData.json"), jsonData);
		}

		internal static void LoadParamsData(MethodInfo[] functions, object target, ref Dictionary<MethodInfo, bool> foldouts, ref Dictionary<MethodInfo, object[]> parameterValues)
		{
			if (!Directory.Exists(PARAMS_DATA_LOCATION)) 
				Directory.CreateDirectory(PARAMS_DATA_LOCATION);

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

		internal static string GetFunctionID(MethodInfo function, object target) => $"{target}_{function.Name}_{string.Join("_", function.GetParameters().Select(param => param.ParameterType.Name))}";

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

		public static T ParseFromJson<T>(object value)
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
		#endregion

		#region ELSE_IF_CHAINS
		internal static VisualElement DrawParameterField(Type fieldType, string fieldName, object fieldValue)
		{
			fieldName = ObjectNames.NicifyVariableName(fieldName);

			if (fieldType == typeof(string))
			{
				return new TextField(fieldName) { value = (string)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(int))
			{
				return new IntegerField(fieldName) { value = (int)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(uint))
			{
				return new UnsignedIntegerField(fieldName) { value = (uint)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(long))
			{
				return new LongField(fieldName) { value = (long)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(ulong))
			{
				return new UnsignedLongField(fieldName) { value = (ulong)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(float))
			{
				return new FloatField(fieldName) { value = (float)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(double))
			{
				return new DoubleField(fieldName) { value = (double)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(bool))
			{
				return new Toggle(fieldName) { value = (bool)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType.IsEnum)
			{
				return new EnumField(fieldName, (Enum)ConvertParameterValue(fieldType, fieldValue));
			}
			else if (fieldType == typeof(Vector2))
			{
				return new Vector2Field(fieldName) { value = (Vector2)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Vector2Int))
			{
				return new Vector2IntField(fieldName) { value = (Vector2Int)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Vector3))
			{
				return new Vector3Field(fieldName) { value = (Vector3)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Vector3Int))
			{
				return new Vector3IntField(fieldName) { value = (Vector3Int)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Vector4))
			{
				return new Vector4Field(fieldName) { value = (Vector4)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Color))
			{
				return new ColorField(fieldName) { value = (Color)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Gradient))
			{
				return new GradientField(fieldName) { value = (Gradient)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				return new CurveField(fieldName) { value = (AnimationCurve)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(LayerMask))
			{
				return new LayerMaskField(fieldName, (LayerMask)ConvertParameterValue(fieldType, fieldValue));
			}
			else if (fieldType == typeof(Rect))
			{
				return new RectField(fieldName) { value = (Rect)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(RectInt))
			{
				return new RectIntField(fieldName) { value = (RectInt)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(Bounds))
			{
				return new BoundsField(fieldName) { value = (Bounds)ConvertParameterValue(fieldType, fieldValue) };
			}
			else if (fieldType == typeof(BoundsInt))
			{
				return new BoundsIntField(fieldName) { value = (BoundsInt)ConvertParameterValue(fieldType, fieldValue) };
			}
			else
			{
				return new HelpBox($"The type {fieldType} is not supported", HelpBoxMessageType.Error);
			}
		}

		private static void RegisterParameterFieldValueChangedCallback(VisualElement field, Type parameterType, Action<object> valueCallback)
		{
			if (parameterType == typeof(string))
			{
				field.RegisterCallback<ChangeEvent<string>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(int))
			{
				field.RegisterCallback<ChangeEvent<int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(uint))
			{
				field.RegisterCallback<ChangeEvent<uint>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(long))
			{
				field.RegisterCallback<ChangeEvent<long>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(ulong))
			{
				field.RegisterCallback<ChangeEvent<ulong>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(float))
			{
				field.RegisterCallback<ChangeEvent<float>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(double))
			{
				field.RegisterCallback<ChangeEvent<double>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(bool))
			{
				field.RegisterCallback<ChangeEvent<bool>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType.IsEnum)
			{
				field.RegisterCallback<ChangeEvent<Enum>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Vector2))
			{
				field.RegisterCallback<ChangeEvent<Vector2>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Vector2Int))
			{
				field.RegisterCallback<ChangeEvent<Vector2Int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Vector3))
			{
				field.RegisterCallback<ChangeEvent<Vector3>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Vector3Int))
			{
				field.RegisterCallback<ChangeEvent<Vector3Int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Vector4))
			{
				field.RegisterCallback<ChangeEvent<Vector4>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Color))
			{
				field.RegisterCallback<ChangeEvent<Color>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Gradient))
			{
				field.RegisterCallback<ChangeEvent<Gradient>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(AnimationCurve))
			{
				field.RegisterCallback<ChangeEvent<AnimationCurve>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(LayerMask))
			{
				field.RegisterCallback<ChangeEvent<int>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Rect))
			{
				field.RegisterCallback<ChangeEvent<Rect>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(RectInt))
			{
				field.RegisterCallback<ChangeEvent<RectInt>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(Bounds))
			{
				field.RegisterCallback<ChangeEvent<Bounds>>((callback) => valueCallback.Invoke(callback.newValue));
			}
			else if (parameterType == typeof(BoundsInt))
			{
				field.RegisterCallback<ChangeEvent<BoundsInt>>((callback) => valueCallback.Invoke(callback.newValue));
			}
		}

		private static object ConvertParameterValue(Type parameterType, object parameterValue)
		{
			bool isDBNull = Convert.IsDBNull(parameterValue);

			if (parameterType == typeof(string))
			{
				return parameterValue?.ToString();
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
		#endregion
	}
}
