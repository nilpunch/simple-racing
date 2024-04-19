using UnityEngine;

public class PurePursuitVehicle : MonoBehaviour
{
	[SerializeField] private VehicleController _vehicleController;
	[SerializeField] private WaypointsRoute _waypointsRoute;
	[SerializeField] private float _lookaheadRadius = 3f;
	[SerializeField] private float _wheelBase = 3f;
	[SerializeField, Range(0f, 1f)] private float _throttle = 0.5f;

	public const float HalfPI = Mathf.PI / 2f;
	public const float TwoPI = Mathf.PI * 2f;
	
	private void Update()
	{
		var transform = _vehicleController.transform;

		var lookaheadPosition = _waypointsRoute.GetLookaheadPosition(transform.position, _lookaheadRadius);

		Vector3 lookaheadPivot = transform.position + transform.rotation * Vector3.forward;
		Vector3 lookToAhead = lookaheadPosition - lookaheadPivot;
		float lookaheadHeading = HalfPI - Mathf.Atan2(lookToAhead.z, lookToAhead.x);
		float vehicleHeading = Mathf.Deg2Rad * transform.eulerAngles.y;
				
		float deltaHeading = (lookaheadHeading - vehicleHeading) % TwoPI;
		if (deltaHeading <= -Mathf.PI)
		{
			deltaHeading += TwoPI;
		}
		else if (deltaHeading > Mathf.PI)
		{
			deltaHeading -= TwoPI;
		}
				
		float targetSteering = Mathf.Clamp(
			Mathf.Atan2(2f * _wheelBase * Mathf.Sin(Mathf.Clamp(deltaHeading, -HalfPI, HalfPI)) / lookToAhead.magnitude, 1f) * 57.29578f,
			-_vehicleController.MaxSteeringAngle,
			_vehicleController.MaxSteeringAngle);

		_vehicleController.SteeringInput = targetSteering / _vehicleController.MaxSteeringAngle;
		_vehicleController.ThrottleInput = _throttle;
	}
}