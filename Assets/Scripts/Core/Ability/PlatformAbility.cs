using PlayerControllers;
using UnityEngine;

public class PlatformAbility : DirectionAbility
{
	private BigBoy _bigBoy;

	public PlatformAbility(Transform playerTransform, DirectionView directionView, BigBoy bigBoy, float radius) : base(playerTransform, directionView, radius)
	{
		_bigBoy = bigBoy;
	}

	public override void Use(Vector2 lookPosition)
	{
		base.Use(lookPosition);
		var bigBoy = GameObject.Instantiate(_bigBoy, _directionView.transform.position, Quaternion.identity);
	}
}