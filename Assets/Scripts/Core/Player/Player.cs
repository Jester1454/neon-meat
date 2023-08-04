using System;
using System.Collections.Generic;
using Input;
using PlayerControllers;
using Sound;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
	[Header("Jump parametrs")] public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float maxJumpWidth;
	public float timeToJumpApex = .4f;
	public float _coyoteTime = .2f;
	public float _jumpBuffer = .2f;
	public AudioClip _jumpClip;
	public AudioClip _onGroundedClip;
	public ParticleSystem _groundedEffect;
	
	[Header("Movement parametrs")] public float MoveSpeed = 6;
	public float accelerationTimeAirborne = .2f;
	public float accelerationTimeGrounded = .2f;

	[Header("Wall climbing parametrs")] public Vector2 wallJumpClimb;
	public Vector2 wallLeap;
	public float wallSlideSpeedMax = 3;
	
	[Header("new shit")]
	[Header("smol")]
	[SerializeField] private DirectionView _smolDirection;
	[SerializeField] private float _smolDirectionRadius;
	[SerializeField] private Smol _smol;
	
	[Header("Big boy")]
	[SerializeField] private DirectionView _bigBoyDirection;
	[SerializeField] private float _bigBoyDirectionRadius;
	[SerializeField] private BigBoy _bigBoy;

	private float gravity;
	private float maxJumpVelocity;
	private float maxJumpWidthVelocity;
	private float minJumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;
	private float _coyteTimeCounter;
	private float _jumpBufferCounter;
	private bool _isGrounded = false;
	private bool _isWalled;
	
	private Controller2D controller;

	private Vector2 directionalInput;
	private bool wallSliding;
	private int wallDirX;

	private float jumpDuration = 0;
	private BaseInputDriver _inputDriver;
	
	private Stack<AbilityType> _currentAbilities = new Stack<AbilityType>();

	public Stack<AbilityType> CurrentAbilities => _currentAbilities;

	public Action OnAbilityChanges;
	
	void Start()
	{
		controller = GetComponent<Controller2D>();
		_inputDriver = GetComponent<NewInputDriver>();
		
		gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
			
		wallSlideSpeedMax = gravity + wallSlideSpeedMax;
	}

	void Update()
	{
		UpdateInputs();

		CalculateVelocity();

		HandleWallSliding();

		controller.Move(velocity * Time.deltaTime, directionalInput);

		if (controller.collisions.above || controller.collisions.below)
		{
			if (controller.collisions.slidingDownMaxSlope)
			{
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = 0;
			}
		}

		UpdateAbilityInput();
	}

	private void UpdateAbilityInput()
	{
		var hasAvailableAbility = _currentAbilities.Count != 0;
		if (!hasAvailableAbility) return;
		
		var nextAbility = _currentAbilities.Peek();
		var lookPosition = _inputDriver.LookPosition;

		if (_inputDriver.HoldingAbility)
		{
			switch (nextAbility)
			{
				case AbilityType.Smol:
					UpdateSmolDirection(lookPosition);
					break;
				case AbilityType.BigBoy:
					UpdateBigBoyDirection(lookPosition);
					break;
			}
		}
		
		if (_inputDriver.ReleaseAbility)
		{
			switch (nextAbility)
			{
				case AbilityType.Smol:
					ShootSmol(lookPosition);
					break;
				case AbilityType.BigBoy:
					SpawnBigBoy();
					break;
			}
		}
	}

	private void UpdateInputs()
	{
		directionalInput = _inputDriver.Movement;

		var isBelow = controller.collisions.below;
		if (isBelow)
		{
			_coyteTimeCounter = _coyoteTime;

			if (_isGrounded != isBelow)
			{
				SoundManager.Instance.Play(_onGroundedClip);
				_groundedEffect.Play();
			}
		}
		else
		{
			_coyteTimeCounter -= Time.deltaTime;
		}

		if (_inputDriver.Jump)
		{
			_jumpBufferCounter = _jumpBuffer;
		}
		else
		{
			_jumpBufferCounter -= Time.deltaTime;
		}
		
		if (_jumpBufferCounter > 0)
		{
			OnJumpInputDown();
		}

		if (_inputDriver.ReleaseJump)
		{
			OnJumpInputUp();
		}
		
		_isGrounded = isBelow;
	}

	private bool CompareSognOfNumber(float num1, float num2)
	{
		if (num1 > 0 && num2 > 0)
		{
			return true;
		}
		else
		{
			if (num1 < 0 && num2 < 0)
			{
				return true;
			}
		}

		return false;
	}

	private void OnJumpInputDown()
	{
		if (wallSliding)
		{
			SoundManager.Instance.Play(_jumpClip);
			if (CompareSognOfNumber(wallDirX, directionalInput.x))
			{
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else
			{
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
			_coyteTimeCounter = 0;
			_jumpBufferCounter = 0;
			return;
		}

		if (_coyteTimeCounter > 0)
		{
			maxJumpWidthVelocity = maxJumpWidth / timeToJumpApex;

			if (controller.collisions.slidingDownMaxSlope)
			{
				if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
				{
					SoundManager.Instance.Play(_jumpClip);

					// not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					//velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
					velocity.x = _inputDriver.Movement.x * maxJumpWidthVelocity * controller.collisions.slopeNormal.x;
				}
			}
			else
			{
				SoundManager.Instance.Play(_jumpClip);
				velocity.y = maxJumpVelocity;
				velocity.x = _inputDriver.Movement.x * maxJumpWidthVelocity;
			}

			_coyteTimeCounter = 0;
			_jumpBufferCounter = 0;
		}
	}

	private void OnJumpInputUp()
	{
		if (velocity.y > minJumpVelocity)
		{
			velocity.y = minJumpVelocity;
		}
	}

	void HandleWallSliding()
	{
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below)
		{
			wallSliding = true;

			if (_isWalled != wallSliding)
			{
				SoundManager.Instance.Play(_onGroundedClip);
			}
			
			if (velocity.y < wallSlideSpeedMax)
			{
				velocity.y = wallSlideSpeedMax;
			}
		}

		_isWalled = wallSliding;
	}

	void CalculateVelocity()
	{
		float targetVelocityX = directionalInput.x * MoveSpeed;

		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
			(controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
	}
	
	private void UpdateSmolDirection(Vector2 lookPosition)
	{
		if (_bigBoyDirection.gameObject.activeSelf)
		{
			_bigBoyDirection.gameObject.SetActive(false);
		}
		UpdateDirection(lookPosition, _smolDirection.transform, _smolDirectionRadius);
		
		_smolDirection.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f,
			Mathf.Atan2(lookPosition.normalized.y, lookPosition.normalized.x) * Mathf.Rad2Deg - 90));	
	}

	private void UpdateBigBoyDirection(Vector2 lookPosition)
	{
		if (_smolDirection.gameObject.activeSelf)
		{
			_smolDirection.gameObject.SetActive(false);
		}
		UpdateDirection(lookPosition, _bigBoyDirection.transform, _bigBoyDirectionRadius);
	}
	
	private void UpdateDirection(Vector2 lookPosition, Transform direction, float radius)
	{
		if (!(lookPosition.magnitude > 0.001f)) return;
			
		direction.position = (Vector2) transform.position + 
		                     lookPosition.normalized * radius;
		if (!direction.gameObject.activeSelf)
		{
			direction.gameObject.SetActive(true);
		}
	}

	private void ShootSmol(Vector2 lookPosition)
	{
		if (_smolDirection.gameObject.activeSelf)
		{
			_smolDirection.gameObject.SetActive(false);
		}
		
		var smol = Instantiate(_smol, _smolDirection.transform.position, Quaternion.identity);
		smol.StartShot(lookPosition.normalized);
		_currentAbilities.Pop();
		OnAbilityChanges?.Invoke();
	}

	private void SpawnBigBoy()
	{
		if (_bigBoyDirection.gameObject.activeSelf)
		{
			_bigBoyDirection.gameObject.SetActive(false);
		}
		
		var bigBoy = Instantiate(_bigBoy, _bigBoyDirection.transform.position, Quaternion.identity);
		_currentAbilities.Pop();
		OnAbilityChanges?.Invoke();
	}

	public void PickUpAbility(AbilityType abilityType)
	{
		_currentAbilities.Push(abilityType);
		OnAbilityChanges?.Invoke();
	}
}