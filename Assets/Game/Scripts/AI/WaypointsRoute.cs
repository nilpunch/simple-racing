using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsRoute : MonoBehaviour
{
	[SerializeField] private Transform[] _waypoints;
	[SerializeField] private bool _isLooped = true;

	public Vector3 GetLookaheadPosition(Vector3 carPosition, float lookaheadRadius)
	{
		const int maxSearchDepth = 8;

		var closest = GetClosestPoint(carPosition);

		Vector3 lastPosition = _waypoints[closest.IndexFirst].position;
		Vector3 lastDirection = Vector3.Normalize(_waypoints[closest.IndexSecond].position - lastPosition);

		Vector3 lookaheadPosition = lastPosition;

		for (int i = 0; i < maxSearchDepth; i++)
		{
			Vector3 nextPosition = _waypoints[(i + closest.IndexSecond) % _waypoints.Length].position;
			Vector3 direction = Vector3.Normalize(nextPosition - lastPosition);

			// Check for sharp turn
			if (Vector3.Dot(lastDirection, direction) < -0.3f)
			{
				break;
			}
			lastDirection = direction;

			(Vector2 Position, float Fraction)? lineCircleIntersection =
				PurePursuitUtils.LineCircleIntersection(lastPosition.ToXZ(), nextPosition.ToXZ(), carPosition.ToXZ(), lookaheadRadius);

			if (lineCircleIntersection.HasValue)
			{
				lookaheadPosition = Vector3.Lerp(lastPosition, nextPosition, lineCircleIntersection.Value.Fraction);
			}

			lastPosition = nextPosition;
		}

		if (Vector2.Distance(lastPosition.ToXZ(), carPosition.ToXZ()) < lookaheadRadius)
		{
			return lastPosition;
		}

		return lookaheadPosition;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		int segmentsCount = _waypoints.Length - 1;
		if (_isLooped)
		{
			segmentsCount += 1;
		}

		for (int i = 0; i < segmentsCount; i++)
		{
			Vector3 lineStart = _waypoints[i % _waypoints.Length].position;
			Vector3 lineEnd = _waypoints[(i + 1) % _waypoints.Length].position;

			Gizmos.DrawLine(lineStart, lineEnd);
		}
	}

	public (int IndexFirst, int IndexSecond, float Fraction) GetClosestPoint(Vector3 vehiclePosition)
	{
		int closestIndexFirst = -1;
		int closestIndexSecond = -1;
		float closestFraction = 0f;
		float closestDistanceSqr = float.MaxValue;

		IReadOnlyList<Transform> path = _waypoints;

		int segmentsCount = path.Count - 1;
		if (_isLooped)
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