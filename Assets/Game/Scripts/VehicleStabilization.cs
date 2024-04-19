using UnityEngine;

public class VehicleStabilization : MonoBehaviour
{
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private float _downThrust = 10f;
	[SerializeField] private float _stabilizationThrust = 10f;

	private const float PointsOffset = 2f;
	
	private void FixedUpdate()
	{
		var transform = _rigidbody.transform;

		_rigidbody.AddForce(Vector3.down * _downThrust, ForceMode.Acceleration);

		var pointCentral = transform.position;
		var point1 = transform.TransformPoint(new Vector3(PointsOffset, 0f, PointsOffset));
		var point2 = transform.TransformPoint(new Vector3(-PointsOffset, 0f, PointsOffset));
		var point3 = transform.TransformPoint(new Vector3(PointsOffset, 0f, -PointsOffset));
		var point4 = transform.TransformPoint(new Vector3(-PointsOffset, 0f, -PointsOffset));

		_rigidbody.AddForceAtPosition(Vector3.up * (pointCentral.y - point1.y) * _stabilizationThrust, point1, ForceMode.Acceleration);
		_rigidbody.AddForceAtPosition(Vector3.up * (pointCentral.y - point2.y) * _stabilizationThrust, point2, ForceMode.Acceleration);
		_rigidbody.AddForceAtPosition(Vector3.up * (pointCentral.y - point3.y) * _stabilizationThrust, point3, ForceMode.Acceleration);
		_rigidbody.AddForceAtPosition(Vector3.up * (pointCentral.y - point4.y) * _stabilizationThrust, point4, ForceMode.Acceleration);
	}
}