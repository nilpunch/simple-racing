using System;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
	[SerializeField] private WheelCollider _frontLeft;
	[SerializeField] private WheelCollider _frontRight;
	[SerializeField] private WheelCollider _backLeft;
	[SerializeField] private WheelCollider _backRight;

	[Space, SerializeField, Range(10f, 80f)] private float _maxSteeringAngle = 50;
	[SerializeField] private float _steeringChangeSpeed = 20;

	[Space, SerializeField, Min(0f)] private float _maxMotorTorque = 30f;
	[SerializeField, Min(0f)] private float _maxBrakesBrakes = 30f;

	private float _currentSteeringAngle;

	public float SteeringInput { get; set; }
	public float ThrottleInput { get; set; }
	public float BrakesInput { get; set; }

	private void FixedUpdate()
	{
		_currentSteeringAngle = Mathf.MoveTowardsAngle(_currentSteeringAngle,
			_maxSteeringAngle * SteeringInput, _steeringChangeSpeed * Time.deltaTime);
		
		_frontLeft.steerAngle = _currentSteeringAngle;
		_frontRight.steerAngle = _currentSteeringAngle;

		_backLeft.motorTorque = _maxMotorTorque * ThrottleInput;
		_backRight.motorTorque = _maxMotorTorque * ThrottleInput;

		float brakesInput = BrakesInput;
		
		// Brake when input direction is not the same as vehicle velocity
		float vehicleForwardVelocity = GetCurrentForwardVelocity();
		if (Math.Sign(vehicleForwardVelocity) != Math.Sign(ThrottleInput) && Math.Abs(vehicleForwardVelocity) >= 0.1f)
		{
			brakesInput = 1f;
		}

		_frontLeft.brakeTorque = _maxBrakesBrakes * brakesInput;
		_frontRight.brakeTorque = _maxBrakesBrakes * brakesInput;
		_backLeft.brakeTorque = _maxBrakesBrakes * brakesInput;
		_backRight.brakeTorque = _maxBrakesBrakes * brakesInput;
	}

	private float GetCurrentForwardVelocity()
	{
		var vehicleRigidbody = _frontLeft.attachedRigidbody;
		var worldVelocity = vehicleRigidbody.velocity;
		var localVelocity = vehicleRigidbody.transform.InverseTransformDirection(worldVelocity);
		return localVelocity.z;
	}
}