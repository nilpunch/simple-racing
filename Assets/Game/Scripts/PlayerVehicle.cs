using System;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour
{
	[SerializeField] private VehicleController _vehicleController;

	private void Update()
	{
		// Throttle
		float throttleInputSum = 0f;
		if (Input.GetKey(KeyCode.W))
		{
			throttleInputSum += 1f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			throttleInputSum -= 1f;
		}
		_vehicleController.ThrottleInput = throttleInputSum;

		// Steering
		float steeringInputSum = 0f;
		if (Input.GetKey(KeyCode.D))
		{
			steeringInputSum += 1f;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			steeringInputSum -= 1f;
		}
		_vehicleController.SteeringInput = steeringInputSum;

		// Brakes
		if (Input.GetKey(KeyCode.Space))
		{
			_vehicleController.BrakesInput = 1f;
		}
		else
		{
			_vehicleController.BrakesInput = 0f;
		}
	}
}