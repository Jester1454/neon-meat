using PlayerControllers;
using Sound;
using UnityEngine;
using UnityEngine.UI;

public class LaserAbility : DirectionAbility
{
	private readonly Image _durationStatus;
	private float _currentTime;
	private readonly float _duration;
	private readonly AbilityController _abilityController;
	private readonly AudioClip _audioClip;
	private bool _isStart;
	
	public LaserAbility(Transform playerTransform, DirectionView directionView, float directionRadius, float duration, AbilityController abilityController, AudioClip audioClip) : base(playerTransform, directionView, directionRadius)
	{
		_duration = duration;
		_abilityController = abilityController;
		_audioClip = audioClip;
		_durationStatus = directionView.GetComponentInChildren<Image>();
	}

	public override void Update(float deltaTime, Vector2 lookPosition)
	{
		base.Update(deltaTime, lookPosition);

		if (!_isStart)
		{
			_isStart = true;
			_currentTime = _duration;
			SoundManager.Instance.PlayLoop(_audioClip);
		}
		
		_currentTime -= deltaTime;
		_durationStatus.fillAmount = _currentTime / _duration;

		if (_currentTime < 0)
		{
			_abilityController.UseCurrentAbility();
		}
	}

	protected override void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		_directionView.transform.rotation = Quaternion.Euler(new Vector3(Mathf.Atan2(lookPosition.normalized.x, lookPosition.normalized.y) * Mathf.Rad2Deg - 90, 90f, 0));	
		
		base.UpdateDirection(lookPosition, direction, radius);
	}
	
	public override void Cancel()
	{
		base.Cancel();
		SoundManager.Instance.StopLoop();
		_isStart = false;
	}

	public override void Use(Vector2 lookPosition)
	{
		base.Use(lookPosition);
		SoundManager.Instance.StopLoop();
		_isStart = false;
	}
}