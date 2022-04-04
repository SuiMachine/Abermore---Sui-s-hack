using UnityEngine;

namespace AbemoreSuisHack
{
	public class Extensions
	{
	}

	public class Vector3Ext
	{
		public static Vector3 Average(params Vector3[] Vectors)
		{
			Vector3 avg = Vector3.zero;
			foreach (var vector in Vectors)
			{
				avg += vector;
			}
			avg /= Vectors.Length;
			return avg;
		}
	}
}
