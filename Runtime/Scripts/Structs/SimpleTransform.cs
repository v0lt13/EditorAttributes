using System;
using UnityEngine;

namespace EditorAttributes
{
	[Serializable]
	public struct SimpleTransform
    {
		public Vector3 position; 
		public Vector3 rotation;
		public Vector3 scale;

		public readonly Quaternion QuaternionRotation => Quaternion.Euler(rotation);

		/// <summary>
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute or for data serialization
		/// </summary>
		/// <param name="position">Input position</param>
		/// <param name="rotation">Input rotation in angles</param>
		/// <param name="scale">Input scale</param>
		public SimpleTransform(Vector3 position, Vector3 rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation;
			this.scale = scale;
		}

		/// <summary>
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute or for data serialization
		/// </summary>
		/// <param name="position">Input position</param>
		/// <param name="rotation">Input rotation</param>
		/// <param name="scale">Input scale</param>
		public SimpleTransform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.position = position;
			this.rotation = rotation.eulerAngles;
			this.scale = scale;
		}

		/// <summary>
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute or for data serialization
		/// </summary>
		/// <param name="transform">Input transform</param>
		public SimpleTransform(Transform transform)
		{
			position = transform.position;
			rotation = transform.eulerAngles;
			scale = transform.localScale;
		}

		public static implicit operator SimpleTransform(Transform transform) => new(transform);

		public override readonly string ToString() => $"{position}, {rotation}, {scale}";

		/// <summary>
		/// Puts the SimpleTransform values to into a Transform
		/// </summary>
		/// <param name="transform">The transform to put the values into</param>
		/// <returns>The transform with the new values</returns>
		public readonly Transform ToTransform(Transform transform)
		{		
			transform.position = position;
			transform.eulerAngles = rotation;
			transform.localScale = scale;

			return transform;
		}
	}
}
