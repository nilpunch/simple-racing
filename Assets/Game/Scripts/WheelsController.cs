using UnityEngine;

public class WheelsController : MonoBehaviour
{
	[SerializeField] private WheelCollider _frontLeft;
	[SerializeField] private WheelCollider _frontRight;
	[SerializeField] private WheelCollider _backLeft;
	[SerializeField] private WheelCollider _backRight;

	[SerializeField, Range(-50f, 50f)] private float _steering;
	[SerializeField, Range(-30f, 30f)] private float _thrust;
	[SerializeField, Range(0f, 100f)] private float _brakes;

	private void Update()
	{
		_frontLeft.steerAngle = _steering;
		_frontRight.steerAngle = _steering;

		_backLeft.motorTorque = _thrust;
		_backRight.motorTorque = _thrust;

		_frontLeft.brakeTorque = _brakes;
		_frontRight.brakeTorque = _brakes;
		_backLeft.brakeTorque = _brakes;
		_backRight.brakeTorque = _brakes;
	}
}