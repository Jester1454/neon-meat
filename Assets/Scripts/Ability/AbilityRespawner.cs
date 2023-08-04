using System;
using System.Collections;
using UnityEngine;

namespace PlayerControllers
{
	public class AbilityRespawner : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPosition;
		[SerializeField] private bool _spawnOnStart;
		[SerializeField] private GameObject _spawnObject;
		[SerializeField] private float _spawnCooldown;
		[SerializeField] private bool _useSpawnLimit;
		[SerializeField] private int _spawnCount;
		
		private GameObject _currentAbility;
		private int _spawnedCount = 0;
		private void Awake()
		{
			if (_spawnOnStart)
			{
				Spawn();
			}
		}

		private void Spawn()
		{
			if (_useSpawnLimit)
			{
				if (_spawnedCount >= _spawnCount)
				{
					return;
				}
			}
			_currentAbility = Instantiate(_spawnObject, _spawnPosition.position, Quaternion.identity);
			_spawnedCount++;
			var deadable = _currentAbility.GetComponent<IDeadable>();
			if (deadable != null)
			{
				deadable.OnDead += OnDeadCurrentAbility;
			}
		}

		private void OnDeadCurrentAbility()
		{
			StartCoroutine(WaitCooldown());
		}

		private IEnumerator WaitCooldown()
		{
			yield return new WaitForSeconds(_spawnCooldown);
			Spawn();
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position, Vector3.one);
		}
	}
}