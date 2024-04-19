using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
	[SerializeField] private Vector3 _centerOfMassOffset = Vector3.zero;

	private Vector3 _centerOfMass;
	private Rigidbody _rigidBody;

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody>();
		_centerOfMass = _rigidBody.centerOfMass;
		UpdateCenterOfMass();
	}

	private void UpdateCenterOfMass()
	{
		_rigidBody.centerOfMass = _centerOfMass + _centerOfMassOffset;
	}

	private void OnDrawGizmosSelected()
	{
		var rigidbody = GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(rigidbody.transform.TransformPoint(rigidbody.centerOfMass + _centerOfMassOffset), 0.1f);
		}
	}
}