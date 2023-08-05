using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayerControllers
{
	public class InstaTrap : MonoBehaviour
	{
		[SerializeField] private GameObject _vfx;
		[SerializeField] private float _vfxDelay = 0.2f;
		private bool _isDead;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isDead) return;
			if (other == null) return;

			var player = other.GetComponent<Player>();

			if (player != null)
			{
				_isDead = true;
				StartCoroutine(WaitToSkip(player.transform.position));
				Destroy(player.gameObject);
			}
		}
		
		
		private IEnumerator WaitToSkip(Vector3 position)
		{
			var vfx = Instantiate(_vfx, position, Quaternion.identity);

			yield return new WaitForSeconds(_vfxDelay);
			Destroy(vfx);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}