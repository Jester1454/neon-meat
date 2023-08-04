using System;
using Sound;
using UnityEngine;

namespace PlayerControllers
{
	public interface IDeadable
	{
		Action OnDead { get; set; }
	}
	
	public class Smol : MonoBehaviour , IDeadable
	{
		[SerializeField] protected float _colliderRadius;
		[SerializeField] protected float _rotationAngleOffset;
		[SerializeField] protected GameObject _vfx;
		[SerializeField] protected float _destroyDelay = 1f;
		[SerializeField] private float _range;
		[SerializeField] private float _speed;
		[SerializeField] private LayerMask _layerMask;
		[SerializeField] private AudioClip _spawn;
		
		[SerializeField] private AudioClip _destroy;
		public Action OnDead { get; set; }
		protected Vector2 _velocity = Vector2.zero;
		protected readonly Collider2D[] _hitColliders = new Collider2D[15];
		protected GameObject _sourceObject;

		public virtual void StartShot(Vector2 direction)
		{
			SoundManager.Instance.Play(_spawn);

			var bulletLifeTime = _range / (direction.normalized.magnitude * _speed);
			_velocity = direction * _speed;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.normalized.y,
				                                 
				                                                        direction.normalized.x) * Mathf.Rad2Deg - _rotationAngleOffset));
		}
		
		protected virtual void Update()
		{
			if (CheckObstacles(transform.position)) return;
			Movement(transform.position);
		}

		protected virtual void Movement(Vector2 currentPosition)
		{
			Vector2 newPosition = currentPosition + _velocity * Time.deltaTime;
			transform.position = newPosition;
		}

		protected virtual bool CheckObstacles(Vector2 currentPosition)
		{
			Physics2D.OverlapCircleNonAlloc(currentPosition, _colliderRadius, _hitColliders, _layerMask);

			foreach (var hit in _hitColliders)
			{
				if (hit == null || hit.gameObject == null) break;
				if (hit.gameObject == this.gameObject) continue;
				if (_sourceObject != null && hit.gameObject == _sourceObject) continue;
				if (hit.gameObject.GetComponent<Smol>()) continue;
				ObstacleHit(hit.gameObject);
				
				return true;
			}

			return false;
		}

		protected virtual void ObstacleHit(GameObject hit)
		{
			var switchObject = hit.gameObject.gameObject.GetComponent<ISwitch>();

			if (switchObject != null)
			{
				switchObject.Switch();
			}

			HitAnimation();
		}

		protected virtual void HitAnimation()
		{
			if (_vfx != null)
			{
				var vfx = Instantiate(_vfx, transform.position, Quaternion.identity);
				Destroy(vfx, _destroyDelay);
			}

			OnDead?.Invoke();
			Destroy(gameObject);
			SoundManager.Instance.Play(_destroy);
		}

		protected virtual void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, _colliderRadius);
		}
	}
}