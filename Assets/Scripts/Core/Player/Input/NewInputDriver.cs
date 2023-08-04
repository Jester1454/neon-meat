using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
	public class NewInputDriver : BaseInputDriver
	{
		private PlayerControls _playerControls;
		private PlayerInput _playerInput;

		public PlayerControls PlayerControls => _playerControls;

		private void Awake()
		{
			_playerControls = new PlayerControls();
			_playerInput = FindObjectOfType<PlayerInput>();
		}

		private void OnEnable()
		{
			_playerControls.Enable();
		}

		private void OnDisable()
		{
			_playerControls.Disable();
		}
		
		private void Update()
		{
			UpdateInput(Time.deltaTime);
		}

		public override void UpdateInput(float timeStep)
		{
			Movement = _playerControls.Gameplay.Movement.ReadValue<Vector2>();
			UpdateLookPosition();
			Jump = _playerControls.Gameplay.Jump.WasPressedThisFrame();
			HoldingJump = _playerControls.Gameplay.Jump.IsPressed();
			ReleaseJump = _playerControls.Gameplay.Jump.WasReleasedThisFrame();

			HoldingAbility = _playerControls.Gameplay.Smoll.WasPerformedThisFrame() || _playerControls.Gameplay.Smoll.IsPressed();;
			ReleaseAbility = _playerControls.Gameplay.Smoll.WasReleasedThisFrame();

			Rerorll = _playerControls.Gameplay.Reroll.WasPerformedThisFrame();
		}
		
				
		private void UpdateLookPosition()
		{
			Vector2 newLookPosition;

			if (_playerInput.user.controlScheme?.name == _playerControls.KeyboardMouseScheme.name)
			{
				newLookPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
				newLookPosition -= (Vector2) transform.position;
				newLookPosition.Normalize();
			}
			else
			{
				newLookPosition = Gamepad.current.rightStick.ReadValue();
			}
			
			if (newLookPosition.magnitude > 0.1f)
			{
				LookPosition = newLookPosition;
			}
		}
	}
}