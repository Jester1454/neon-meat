using System;
using Sound;
using UnityEngine;

namespace PlayerControllers
{
	public class PickableAblity : MonoBehaviour, IDeadable
	{
		[SerializeField] private AbilityType _type;
		[SerializeField] private AudioClip _clip;
		
		private bool _isPickedUp;
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isPickedUp) return;
			if (other == null) return;

			var player = other.GetComponent<Player>();

			if (player != null)
			{
				player.PickUpAbility(_type);
				_isPickedUp = true;
				OnDead?.Invoke();
				SoundManager.Instance.Play(_clip);
				Destroy(gameObject);
			}
		}

		public Action OnDead { get; set; }
	}
}