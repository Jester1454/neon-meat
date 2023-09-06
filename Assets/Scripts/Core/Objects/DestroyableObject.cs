using Unity.Mathematics;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
	[SerializeField] private GameObject _vfx;
	[SerializeField] private float _vfxDuration = 0.5f;
	[SerializeField] private GameObject _destroyableObject;
	
	public void Destroy()
	{
		if (_vfx != null)
		{
			var vfx = Instantiate(_vfx, transform.position, quaternion.identity);
			Destroy(vfx, _vfxDuration);	
		}

		Destroy(_destroyableObject != null ? _destroyableObject : gameObject);
	}
}