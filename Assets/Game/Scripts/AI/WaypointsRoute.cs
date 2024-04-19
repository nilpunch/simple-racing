using System.Collections.Generic;
using UnityEngine;

public class WaypointsRoute : MonoBehaviour
{
	[SerializeField] private Transform[] _waypoints;
	[SerializeField] private bool _isLooped = true;
	
	
	
	public static (int IndexFirst, int IndexSecond, float Fraction) GetClosestPoint(IReadOnlyList<Transform> path, Vector3 vehiclePosition, bool isLooped = false)
	{
		int closestIndexFirst = -1;
		int closestIndexSecond = -1;
		float closestFraction = 0f;
		float closestDistanceSqr = float.MaxValue;

		int segmentsCount = path.Count - 1;
		if (isLooped)
		{
			segmentsCount += 1;
		}

		for (int i = 0; i < segmentsCount; i++)
		{
			Vector3 lineStart = path[i % path.Count].position;
			Vector3 lineEnd = path[(i + 1) % path.Count].position;

			Vector3 lineDirection = lineEnd - lineStart;
			float lineLength = lineDirection.magnitude;

			float distanceSqr;
			float fraction;

			if (lineLength <= 0.00001f)
			{
				distanceSqr = Vector3.SqrMagnitude(lineStart - vehiclePosition);
				fraction = 0f;
			}
			else
			{
				Vector3 lineUnitDirection = lineDirection / lineLength;

				Vector3 fromStartToPoint = vehiclePosition - lineStart;

				float dotProduct = Vector3.Dot(fromStartToPoint, lineUnitDirection);
				dotProduct = Mathf.Clamp(dotProduct, 0f, lineLength);

				Vector3 closest = lineStart + lineUnitDirection * dotProduct;

				distanceSqr = Vector3.SqrMagnitude(closest - vehiclePosition);
				fraction = dotProduct / lineLength;
			}

			// Update the closest point if a smaller distance is found
			if (distanceSqr < closestDistanceSqr)
			{
				closestDistanceSqr = distanceSqr;
				closestIndexFirst = i % path.Count;
				closestIndexSecond = (i + 1) % path.Count;
				closestFraction = fraction;
			}
		}

		return (closestIndexFirst, closestIndexSecond, closestFraction);
	}
}