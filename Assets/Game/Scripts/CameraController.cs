using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Camera _camera;
	[SerializeField] private Transform _followTarget;

	[SerializeField] private float _cameraRotationSpeed = 1f;
	[SerializeField] private float _cameraFollowSpeed = 1f;
	[SerializeField] private Vector3 _followOffset = new Vector3(0f, 5f, -10f);
	[SerializeField] private Vector3 _targetOffset = new Vector3(0f, 1f);

	private Vector3 _cameraVelocity;
	private Vector3 _cameraAngularVelocity;
	
	private void LateUpdate()
	{
		var cameraTransform = _camera.transform;

		Vector3 cameraTargetPosition = _followTarget.TransformPoint(_followOffset);
		Vector3 cameraTargetForward = _followTarget.TransformPoint(_targetOffset) - cameraTargetPosition;

		cameraTargetForward = (cameraTargetForward + _followTarget.forward) / 2f;

		float positionBlend = 1f - Mathf.Pow(0.5f, Time.deltaTime * _cameraFollowSpeed);
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTargetPosition, positionBlend);
		
		float rotationBlend = 1f - Mathf.Pow(0.5f, Time.deltaTime * _cameraRotationSpeed);
		cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(cameraTargetForward), rotationBlend);
	}

	[ContextMenu("Align Camera")]
	private void AlignCamera()
	{
		Vector3 cameraTargetPosition = _followTarget.TransformPoint(_followOffset);
		Vector3 cameraTargetForward = _followTarget.TransformPoint(_targetOffset) - cameraTargetPosition;

		_camera.transform.position = cameraTargetPosition;
		_camera.transform.rotation = Quaternion.LookRotation(cameraTargetForward);

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(_camera.transform);
#endif
	}
}