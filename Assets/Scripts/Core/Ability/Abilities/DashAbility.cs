using PlayerControllers;
using UnityEngine;

public class DashAbility : DirectionAbility
{
	private readonly Player _player;
	private readonly float _dashForce;
	private readonly float _dashDuration;

	public DashAbility(Player player, DirectionView dashView, float dashForce, float dashDuration, float radius) : base(player.transform, dashView, radius)
	{
		_player = player;
		_dashForce = dashForce;
		_dashDuration = dashDuration;
	}

	protected override void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		_directionView.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f,
			Mathf.Atan2(lookPosition.normalized.y, lookPosition.normalized.x) * Mathf.Rad2Deg));	
		
		base.UpdateDirection(lookPosition, direction, radius);
	}
	
	public override void Use(Vector2 lookPosition)
	{
		base.Use(lookPosition);
		_player.Dash(_dashDuration, _dashForce, lookPosition.normalized);
	}
}