using PlayerControllers;
using UnityEngine;

public abstract class DirectionAbility : IAbility
{
	protected readonly Transform _playerTransform;
	protected readonly DirectionView _directionView;
	private readonly float _directionRadius;
	
	public DirectionAbility(Transform playerTransform, DirectionView directionView, float directionRadius)
	{
		_playerTransform = playerTransform;
		_directionView = directionView;
		_directionRadius = directionRadius;
	}
	
	public virtual void Update(float deltaTime, Vector2 lookPosition)
	{
		UpdateDirection(lookPosition, _directionView.transform, _directionRadius);
	}

	public virtual void Cancel()
	{
		if (_directionView.gameObject.activeSelf)
		{
			_directionView.gameObject.SetActive(false);
		}
	}

	public virtual void Use(Vector2 lookPosition)
	{
		if (_directionView.gameObject.activeSelf)
		{
			_directionView.gameObject.SetActive(false);
		}
	}

	protected virtual void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		if (!(lookPosition.magnitude > 0.001f)) return;
			
		direction.position = (Vector2) _playerTransform.position + 
		                     lookPosition.normalized * radius;
		if (!direction.gameObject.activeSelf)
		{
			direction.gameObject.SetActive(true);
		}
	}
}