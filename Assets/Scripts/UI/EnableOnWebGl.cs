using UnityEngine;

namespace UI
{
	public class EnableOnWebGl : MonoBehaviour
	{
		[SerializeField] private GameObject _object;
		private void Awake()
		{
#if !UNITY_WEBGL
			_object.SetActive(false);
#endif
		}
	}
}