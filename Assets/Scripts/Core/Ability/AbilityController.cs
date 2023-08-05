using System;
using System.Collections.Generic;
using Input;
using PlayerControllers;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
	[Header("Common direction")]
	[SerializeField] private DirectionView _commonDirection;
	[SerializeField] private float _commonDirectionRadius;
	
	[Space]
	[Header("smol")]
	[SerializeField] private DirectionView _smolDirection;
	[SerializeField] private float _smolDirectionRadius;
	[SerializeField] private Smol _smol;
	[SerializeField] private float _homingDistance;
	[SerializeField] private float _homingAngle;
	
	[Space]
	[Header("Big boy")]
	[SerializeField] private DirectionView _bigBoyDirection;
	[SerializeField] private float _bigBoyDirectionRadius;
	[SerializeField] private BigBoy _bigBoy;

	[Space] 
	[Header("Double jump")]
	[SerializeField] private GameObject _doubleJumpView;
	
	[Space] 
	[Header("Dash")]
	[SerializeField] private GameObject _dashView;
	[SerializeField] private float _dashForce;
	[SerializeField] private float _dashDuration;

	private Stack<IAbility> _currentAbilities = new Stack<IAbility>();
	private IAbility _currentAbility;
	
	public Stack<AbilityType> CurrentAbilities => _currentAbilitieTypes;
	private Stack<AbilityType> _currentAbilitieTypes = new Stack<AbilityType>();

	public Action OnAbilityChanges;
	private BaseInputDriver _inputDriver;
	private CommonDirectionAbility _commonDirectionAbility;
	private Player _player;
	
	private void Start()
	{
		_player = GetComponent<Player>();
		_inputDriver = GetComponent<NewInputDriver>();
		_commonDirectionAbility = new CommonDirectionAbility(transform, _commonDirection, _commonDirectionRadius);
	}

	public void Update()
	{
		UpdateAbilityInput();
	}

	private void UpdateAbilityInput()
	{
		var hasAvailableAbility = _currentAbilitieTypes.Count != 0;
		_commonDirectionAbility.Cancel();

		if (!hasAvailableAbility) return;
		
		var nextAbility = _currentAbilities.Peek();

		if (nextAbility != _currentAbility)
		{
			_currentAbility?.Cancel();
			_currentAbility = nextAbility;
		}
		
		var lookPosition = _inputDriver.LookPosition;

		if (_inputDriver.HoldingAbility)
		{
			nextAbility.Update(Time.deltaTime, lookPosition);
			_commonDirectionAbility.Cancel();
		}
		else
		{
			_commonDirectionAbility.Update(Time.deltaTime, lookPosition);
		}
		
		if (_inputDriver.ReleaseAbility)
		{
			UseAbility(nextAbility, lookPosition);
		}
	}

	private void UseAbility(IAbility ability, Vector2 lookPosition)
	{
		ability.Use(lookPosition);
		_currentAbilities.Pop();
		_currentAbilitieTypes.Pop();
		OnAbilityChanges?.Invoke();
	}

	public void PickUpAbility(AbilityType abilityType)
	{
		var newAbility = CreateAbility(abilityType);
		if (newAbility == null)
		{
			Debug.LogError($"wrong ability pick up type {abilityType}");
			return;
		}
		_currentAbilitieTypes.Push(abilityType);
		_currentAbilities.Push(newAbility);
		OnAbilityChanges?.Invoke();
	}

	private IAbility CreateAbility(AbilityType type)
	{
		switch (type)
		{
			case AbilityType.Smol:
				return new ShotAbility(transform, _smolDirection, _smolDirectionRadius, _smol, _homingAngle, _homingDistance);
			case AbilityType.BigBoy:
				return new PlatformAbility(transform, _bigBoyDirection, _bigBoy, _bigBoyDirectionRadius);
			case AbilityType.DoubleJump:
				return new DoubleJumpAbility(_player, _doubleJumpView);
			case AbilityType.Dash:
				return new DashAbility(_player, _dashView, _dashForce, _dashDuration);
		}

		return null;
	}
}