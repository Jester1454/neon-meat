using UnityEngine;

public class DashAbility : IAbility
{
	private readonly Player _player;
	private readonly GameObject _doubleJumpView;
	private readonly float _dashForce;
	private readonly float _dashDuration;

	public DashAbility(Player player, GameObject doubleJumpView, float dashForce, float dashDuration)
	{
		_player = player;
		_doubleJumpView = doubleJumpView;
		_dashForce = dashForce;
		_dashDuration = dashDuration;
	}
	
	public void Update(float deltaTime, Vector2 lookPosition)
	{
		if (!_doubleJumpView.gameObject.activeSelf)
		{
			_doubleJumpView.gameObject.SetActive(true);
		}
	}

	public void Cancel()
	{
		if (_doubleJumpView.gameObject.activeSelf)
		{
			_doubleJumpView.gameObject.SetActive(false);
		}
	}

	public void Use(Vector2 lookPosition)
	{
		if (_doubleJumpView.gameObject.activeSelf)
		{
			_doubleJumpView.gameObject.SetActive(false);
		}
		_player.Dash(_dashDuration, _dashForce);
	}
}