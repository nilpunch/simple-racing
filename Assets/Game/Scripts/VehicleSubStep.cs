using System;
using UnityEngine;

public class VehicleSubStep : MonoBehaviour
{
	[SerializeField] private WheelCollider _wheel;
	[SerializeField, Min(2)] private int _substeps = 8;

	private void Awake()
	{
		_wheel.ConfigureVehicleSubsteps(5f, _substeps, _substeps);
	}
}