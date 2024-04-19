using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class IgnoreCollision : MonoBehaviour
{
	[SerializeField] private Collider[] _collidersToIgnore;

	private void Awake()
	{
		var colliders = GetComponents<Collider>();

		foreach (Collider currentCollider in colliders)
		{
			foreach (Collider colliderToIgnore in _collidersToIgnore)
			{
				if (currentCollider != colliderToIgnore)
				{
					Physics.IgnoreCollision(currentCollider, colliderToIgnore, true);
				}
			}
		}
	}
}