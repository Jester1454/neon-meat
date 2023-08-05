using Input;
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

	void Start()
	{
		Cursor.visible = false;
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

	public void ForceJump()
	{
		_coyteTimeCounter = _coyoteTime;
		_jumpBufferCounter = _jumpBuffer;
		_groundedEffect.Play();
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
}