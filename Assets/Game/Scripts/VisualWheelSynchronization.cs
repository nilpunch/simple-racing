using UnityEngine;

public class VisualWheelSynchronization : MonoBehaviour
{
	[SerializeField] private WheelCollider _wheel;
	[SerializeField] private Transform _visual;

	private void LateUpdate()
	{
		_wheel.GetWorldPose(out var position, out var rotation);
		_visual.SetPositionAndRotation(position, rotation);
	}

	[ContextMenu("Sync visual wheel")]
	private void SyncVisualWithWheel()
	{
		LateUpdate();

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(_visual);
#endif
	}
}