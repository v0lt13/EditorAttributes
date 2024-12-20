#nullable enable

using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EditorAttributes.Editor.Utility
{
	public class UnityTypeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) =>
			typeof(Vector2).IsAssignableFrom(objectType) || typeof(Vector2Int).IsAssignableFrom(objectType) || typeof(Vector3).IsAssignableFrom(objectType) || typeof(Vector3Int).IsAssignableFrom(objectType) ||
			typeof(Vector4).IsAssignableFrom(objectType) || typeof(Color).IsAssignableFrom(objectType) || typeof(Rect).IsAssignableFrom(objectType) || typeof(RectInt).IsAssignableFrom(objectType) ||
			typeof(Bounds).IsAssignableFrom(objectType) || typeof(BoundsInt).IsAssignableFrom(objectType);

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			var jsonObject = JObject.Load(reader);

			if (objectType == typeof(Vector2))
			{
				var x = jsonObject["x"]?.Value<float>() ?? 0f;
				var y = jsonObject["y"]?.Value<float>() ?? 0f;

				return new Vector2(x, y);
			}
			if (objectType == typeof(Vector2Int))
			{
				var x = jsonObject["x"]?.Value<int>() ?? 0;
				var y = jsonObject["y"]?.Value<int>() ?? 0;

				return new Vector2Int(x, y);
			}
			else if (objectType == typeof(Vector3))
			{
				var x = jsonObject["x"]?.Value<float>() ?? 0f;
				var y = jsonObject["y"]?.Value<float>() ?? 0f;
				var z = jsonObject["z"]?.Value<float>() ?? 0f;

				return new Vector3(x, y, z);
			}
			else if (objectType == typeof(Vector3Int))
			{
				var x = jsonObject["x"]?.Value<int>() ?? 0;
				var y = jsonObject["y"]?.Value<int>() ?? 0;
				var z = jsonObject["z"]?.Value<int>() ?? 0;

				return new Vector3Int(x, y, z);
			}
			else if (objectType == typeof(Vector4))
			{
				var x = jsonObject["x"]?.Value<float>() ?? 0f;
				var y = jsonObject["y"]?.Value<float>() ?? 0f;
				var z = jsonObject["z"]?.Value<float>() ?? 0f;
				var w = jsonObject["w"]?.Value<float>() ?? 0f;

				return new Vector4(x, y, z, w);
			}
			else if (objectType == typeof(Color))
			{
				var r = jsonObject["r"]?.Value<float>() ?? 0f;
				var g = jsonObject["g"]?.Value<float>() ?? 0f;
				var b = jsonObject["b"]?.Value<float>() ?? 0f;
				var a = jsonObject["a"]?.Value<float>() ?? 0f;

				return new Color(r, g, b, a);
			}
			else if (objectType == typeof(Rect))
			{
				var x = jsonObject["x"]?.Value<float>() ?? 0f;
				var y = jsonObject["y"]?.Value<float>() ?? 0f;
				var width = jsonObject["width"]?.Value<float>() ?? 0f;
				var height = jsonObject["height"]?.Value<float>() ?? 0f;

				return new Rect(x, y, width, height);
			}
			else if (objectType == typeof(RectInt))
			{
				var x = jsonObject["x"]?.Value<int>() ?? 0;
				var y = jsonObject["y"]?.Value<int>() ?? 0;
				var width = jsonObject["width"]?.Value<int>() ?? 0;
				var height = jsonObject["height"]?.Value<int>() ?? 0;

				return new RectInt(x, y, width, height);
			}
			else if (objectType == typeof(Bounds))
			{
				var centerX = jsonObject["centerX"]?.Value<float>() ?? 0f;
				var centerY = jsonObject["centerY"]?.Value<float>() ?? 0f;
				var centerZ = jsonObject["centerZ"]?.Value<float>() ?? 0f;
				var sizeX = jsonObject["sizeX"]?.Value<float>() ?? 0f;
				var sizeY = jsonObject["sizeY"]?.Value<float>() ?? 0f;
				var sizeZ = jsonObject["sizeZ"]?.Value<float>() ?? 0f;

				return new Bounds(new(centerX, centerY, centerZ), new(sizeX, sizeY, sizeZ));
			}
			else if (objectType == typeof(BoundsInt))
			{
				var centerX = jsonObject["centerX"]?.Value<int>() ?? 0;
				var centerY = jsonObject["centerY"]?.Value<int>() ?? 0;
				var centerZ = jsonObject["centerZ"]?.Value<int>() ?? 0;
				var sizeX = jsonObject["sizeX"]?.Value<int>() ?? 0;
				var sizeY = jsonObject["sizeY"]?.Value<int>() ?? 0;
				var sizeZ = jsonObject["sizeZ"]?.Value<int>() ?? 0;

				return new BoundsInt(new(centerX, centerY, centerZ), new(sizeX, sizeY, sizeZ));
			}

			return null;
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) 
		{
			if (value is Vector2 vector2)
			{
				var jsonObject = new JObject
				{
					{ "x", vector2.x },
					{ "y", vector2.y }
				};

				jsonObject.WriteTo(writer);
			}
			if (value is Vector2Int vector2int)
			{
				var jsonObject = new JObject
				{
					{ "x", vector2int.x },
					{ "y", vector2int.y }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Vector3 vector3)
			{
				var jsonObject = new JObject
				{
					{ "x", vector3.x },
					{ "y", vector3.y },
					{ "z", vector3.z }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Vector3Int vector3Int)
			{
				var jsonObject = new JObject
				{
					{ "x", vector3Int.x },
					{ "y", vector3Int.y },
					{ "z", vector3Int.z }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Vector4 vector4)
			{
				var jsonObject = new JObject
				{
					{ "x", vector4.x },
					{ "y", vector4.y },
					{ "z", vector4.z },
					{ "w", vector4.w }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Color color)
			{
				var jsonObject = new JObject
				{
					{ "r", color.r },
					{ "g", color.g },
					{ "b", color.b },
					{ "a", color.a }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Rect rect)
			{
				var jsonObject = new JObject
				{
					{ "x", rect.x },
					{ "y", rect.y },
					{ "width", rect.width },
					{ "height", rect.height }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is RectInt rectInt)
			{
				var jsonObject = new JObject
				{
					{ "x", rectInt.x },
					{ "y", rectInt.y },
					{ "width", rectInt.width },
					{ "height", rectInt.height }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is Bounds bounds)
			{
				var jsonObject = new JObject
				{
					{ "centerX", bounds.center.x },
					{ "centerY", bounds.center.y },
					{ "centerZ", bounds.center.z },
					{ "sizeX", bounds.size.x },
					{ "sizeY", bounds.size.y },
					{ "sizeZ", bounds.size.z }
				};

				jsonObject.WriteTo(writer);
			}
			else if (value is BoundsInt boundsInt)
			{
				var jsonObject = new JObject
				{
					{ "centerX", boundsInt.center.x },
					{ "centerY", boundsInt.center.y },
					{ "centerZ", boundsInt.center.z },
					{ "sizeX", boundsInt.size.x },
					{ "sizeY", boundsInt.size.y },
					{ "sizeZ", boundsInt.size.z }
				};

				jsonObject.WriteTo(writer);
			}
			else
			{
				Debug.LogError($"Serialization of type {value?.GetType()} is not supported");
			}
		}
	}
}
