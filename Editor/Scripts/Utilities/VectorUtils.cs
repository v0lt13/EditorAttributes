using System;
using UnityEngine;

namespace EditorAttributes.Editor
{
	public static class VectorUtils
    {
		/// <summary>
		/// Adds a value to a vector
		/// </summary>
		/// <param name="vector">The vector to add the value to</param>
		/// <param name="addend">The value to add</param>
		/// <returns>A vector with the value added to all axis</returns>
		public static Vector3 AddVector(Vector3 vector, float addend) => vector + new Vector3(addend, addend, addend);

		/// <summary>
		/// Subtracts a value from a vector
		/// </summary>
		/// <param name="vector">The vector to subtract the value from</param>
		/// <param name="subtrahend">The value to subtract</param>
		/// <returns>A vector with the value subtracted from all axis</returns>
		public static Vector3 SubtractVector(Vector3 vector, float subtrahend) => vector - new Vector3(subtrahend, subtrahend, subtrahend);

		/// <summary>
		/// Converts a Vector3Int to a Vector2Int
		/// </summary>
		/// <param name="vector3Int">The Vector3Int to convert</param>
		/// <returns>The converted Vector2Int</returns>
		public static Vector2Int Vector3IntToVector2Int(Vector3Int vector3Int) => new(vector3Int.x, vector3Int.y);

		/// <summary>
		/// Converts a Vector2Int to a Vector2
		/// </summary>
		/// <param name="vector2int">The Vector2Int to convert</param>
		/// <returns>The converted Vector2</returns>
		public static Vector2 Vector2IntToVector2(Vector2Int vector2int) => new(vector2int.x, vector2int.y);

		/// <summary>
		/// Converts a Vector2 to a Vector2Int
		/// </summary>
		/// <param name="vector2">The Vector2 to convert</param>
		/// <returns>The converted Vector2Int</returns>
		public static Vector2Int Vector2ToVector2Int(Vector2 vector2) => new((int)vector2.x, (int)vector2.y);

		/// <summary>
		/// Converts a Vector3 to a Vector3Int
		/// </summary>
		/// <param name="vector3">The Vector3 to convert</param>
		/// <returns>The converted Vector3Int</returns>
		public static Vector3Int Vector3ToVector3Int(Vector3 vector3) => new((int)vector3.x, (int)vector3.y, (int)vector3.z);

		/// <summary>
		/// Converts a Bounds to a BoundsInt
		/// </summary>
		/// <param name="bounds">The bounds to convert</param>
		/// <returns>The converted BoundsInt</returns>
		public static BoundsInt BoundsToBoundsInt(Bounds bounds) => new(Vector3ToVector3Int(bounds.center), Vector3ToVector3Int(bounds.extents));

		/// <summary>
		/// Parses a string to a Vector2
		/// </summary>
		/// <param name="vectorString">The string representing the vector</param>
		/// <returns>The vector value from the string</returns>
		public static Vector2 ParseVector2(string vectorString)
		{
			var vector = new Vector2();
			var vectorValues = vectorString.Split(',');

			try
			{
				vector.x = float.Parse(vectorValues[0].Replace("(", ""));
				vector.y = float.Parse(vectorValues[1].Replace(")", ""));
			}
			catch (Exception)
			{
				Debug.LogError($"Failed to parse Vector2 from string: \"{vectorString}\"");
			}

			return vector;
		}

		/// <summary>
		/// Parses a string to a Vector2Int
		/// </summary>
		/// <param name="vectorString">The string representing the vector</param>
		/// <returns>The vector value from the string</returns>
		public static Vector2Int ParseVector2Int(string vectorString) => Vector2ToVector2Int(ParseVector2(vectorString));

		/// <summary>
		/// Parses a string to a Vector3
		/// </summary>
		/// <param name="vectorString">The string representing the vector</param>
		/// <returns>The vector value from the string</returns>
		public static Vector3 ParseVector3(string vectorString)
		{
			var vector = new Vector3();
			var vectorValues = vectorString.Split(',');

			try
			{
				vector.x = float.Parse(vectorValues[0].Replace("(", ""));
				vector.y = float.Parse(vectorValues[1]);
				vector.z = float.Parse(vectorValues[2].Replace(")", ""));
			}
			catch (Exception)
			{
				Debug.LogError($"Failed to parse Vector3 from string: \"{vectorString}\"");
			}

			return vector;
		}

		/// <summary>
		/// Parses a string to a Vector3Int
		/// </summary>
		/// <param name="vectorString">The string representing the vector</param>
		/// <returns>The vector value from the string</returns>
		public static Vector3Int ParseVector3Int(string vectorString) => Vector3ToVector3Int(ParseVector3(vectorString));

		/// <summary>
		/// Parses a string to a Vector4
		/// </summary>
		/// <param name="vectorString">The string representing the vector</param>
		/// <returns>The vector value from the string</returns>
		public static Vector4 ParseVector4(string vectorString)
		{
			var vector = new Vector4();
			var vectorValues = vectorString.Split(',');

			try
			{
				vector.x = float.Parse(vectorValues[0].Replace("(", ""));
				vector.y = float.Parse(vectorValues[1]);
				vector.z = float.Parse(vectorValues[2]);
				vector.w = float.Parse(vectorValues[3].Replace(")", ""));
			}
			catch (Exception)
			{
				Debug.LogError($"Failed to parse Vector4 from string: \"{vectorString}\"");
			}

			return vector;
		}
	}
}
