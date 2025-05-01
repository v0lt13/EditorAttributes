using System;
using UnityEngine;

namespace EditorAttributes
{
	/// <summary>
	/// A simple serializable transform struct that can be used with the DrawHandle Attribute
	/// </summary>
	[Serializable]
	public struct SimpleTransform
    {
		public Vector3 position; 
		public Vector3 rotation;
		public Vector3 scale;

		public readonly Quaternion QuaternionRotation => Quaternion.Euler(rotation);

		/// <summary>
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute
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
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute
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
		/// A simple serializable transform struct that can be used with the DrawHandle Attribute
		/// </summary>
		/// <param name="transform">Input transform</param>
		public SimpleTransform(Transform transform)
		{
			position = transform.position;
			rotation = transform.eulerAngles;
			scale = transform.localScale;
		}

		public static implicit operator SimpleTransform(Transform transform) => new(transform);

		public override readonly string ToString() => $"Position: {position}, Rotation: {rotation}, Scale: {scale}";

		/// <summary>
		/// Puts the SimpleTransform values to into a Transform in world space
		/// </summary>
		/// <param name="transform">The transform to put the values into</param>
		public readonly void ToTransform(Transform transform)
		{
			transform.SetPositionAndRotation(position, QuaternionRotation);
			transform.localScale = scale;
		}

		/// <summary>
		/// Puts the SimpleTransform values to into a Transform in local space
		/// </summary>
		/// <param name="transform">The transform to put the values into</param>
		public readonly void ToLocalTransform(Transform transform)
		{
			transform.SetLocalPositionAndRotation(position, QuaternionRotation);
			transform.localScale = scale;
		}
	}
}
