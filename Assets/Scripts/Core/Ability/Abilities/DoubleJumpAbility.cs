using UnityEngine;

public class DoubleJumpAbility : IAbility
{
	private readonly Player _player;
	private readonly GameObject _doubleJumpView;

	public DoubleJumpAbility(Player player, GameObject doubleJumpView)
	{
		_player = player;
		_doubleJumpView = doubleJumpView;
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
		_player.ForceJump();
	}
}