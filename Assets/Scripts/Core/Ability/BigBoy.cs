using System;
using Sound;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerControllers
{
	public class BigBoy : MonoBehaviour, IDeadable
	{
		[SerializeField] private float _cooldown;
		[SerializeField] private Image _counter;
		[SerializeField] private AudioClip _spawn;
		[SerializeField] private AudioClip _destroy;
		[SerializeField] protected GameObject _vfx;
		[SerializeField] protected float _destroyDelay = 0.5f;
		
		private float _currentTime;
		private void Awake()
		{
			_currentTime = _cooldown;
			Destroy(gameObject, _cooldown);	
			
			SoundManager.Instance.Play(_spawn);
		}
		
		public Action OnDead { get; set; }

		private void OnDestroy()
		{
			SoundManager.Instance.Play(_destroy);

			if (_vfx != null)
			{
				var vfx = Instantiate(_vfx, transform.position, Quaternion.identity);
				Destroy(vfx, _destroyDelay);
			}

			OnDead?.Invoke();
		}

		private void Update()
		{
			_currentTime -= Time.deltaTime;
			_counter.fillAmount = _currentTime / _cooldown;
		}
	}
}