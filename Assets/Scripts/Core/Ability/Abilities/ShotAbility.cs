using PlayerControllers;
using UnityEngine;

public class ShotAbility : DirectionAbility
{
	private readonly Smol _smol;
	private readonly float _homingAngle;
	private readonly float _homingDistance;

	readonly Collider2D[] _targets = new Collider2D[50];

	public ShotAbility(Transform playerTransform, DirectionView directionView, float directionRadius, Smol smol, float homingAngle, float homingDistance) : base(playerTransform, directionView, directionRadius)
	{
		_smol = smol;
		_homingAngle = homingAngle;
		_homingDistance = homingDistance;
	}

	protected override void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		_directionView.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f,
			Mathf.Atan2(lookPosition.normalized.y, lookPosition.normalized.x) * Mathf.Rad2Deg - 90));	
		
		base.UpdateDirection(lookPosition, direction, radius);
	}
	public override void Use(Vector2 lookPosition)
	{
		base.Use(lookPosition);
		lookPosition = GetHomingVector(lookPosition);
		var smol = GameObject.Instantiate(_smol, _directionView.transform.position, Quaternion.identity);
		smol.StartShot(lookPosition.normalized);
	}
	
	private Vector3 GetHomingVector(Vector2 input)
	{
		Physics2D.OverlapCircleNonAlloc(_playerTransform.position, _homingDistance, _targets);

		var minDistance = float.MaxValue;
		var finalVector = input;
		foreach (var target in _targets)
		{
			if (target == null) continue;
			
			var smolSwitch = target.GetComponent<SmolSwitch>();

			if (smolSwitch == null) continue;
			
			if (!IsInHitBox(_playerTransform.position, input, target.transform.position))
				continue;
			
			var direction = target.transform.position - _playerTransform.position;
			if (direction.sqrMagnitude < minDistance)
			{
				minDistance = direction.sqrMagnitude;
				finalVector = direction;
			}
		}

		return finalVector;
	}

	private bool IsInHitBox(Vector2 position, Vector2 forward, Vector2 target)
	{
		var direction = target - position;
		var deltaAngle = Vector2.Angle(direction, forward);
		return !(deltaAngle > _homingAngle);
	}
	
}