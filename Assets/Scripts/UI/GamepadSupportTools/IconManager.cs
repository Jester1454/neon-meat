using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
	public class IconManager : MonoBehaviour
	{
		public IconSwitch[] Switches;
		[Header("Device Schemes from the Input Action Asset")]
		[SerializeField] private string keyboardMouseScheme;
		[SerializeField] private string gamepadScheme;
		private PlayerInput _playerInput;
		
		private void Awake()
		{
			_playerInput = FindObjectOfType<PlayerInput>();
			OnDeviceChange(_playerInput);
			_playerInput.controlsChangedEvent.AddListener(OnDeviceChange);;
		}

		private void OnDeviceChange(PlayerInput playerInput)
		{
			var currentControlScheme = playerInput.currentControlScheme;
			
			if (currentControlScheme == keyboardMouseScheme)
				SwitchIcons(0);
			else if (currentControlScheme == gamepadScheme)
				SwitchIcons(1);
		}

		private void SwitchIcons(int switchTo)
		{
			foreach (IconSwitch currSwitch in Switches)
			{
				foreach (SpriteRenderer currSprite in currSwitch.SwitchableIcons)
				{
					currSprite.sprite = currSwitch.DeviceIcons[switchTo];
				}

				foreach (Image currImage in currSwitch.SwitchableUIImages)
				{
					currImage.sprite = currSwitch.DeviceIcons[switchTo];
				}
				
				foreach (var obj in currSwitch.ActiveObjects)
				{
					obj.KeyboardObject.SetActive(!Convert.ToBoolean(switchTo));
					obj.GamepadObject.SetActive(Convert.ToBoolean(switchTo));
				}
			}
		}

		private void OnDisable()
		{
			_playerInput.controlsChangedEvent.RemoveListener(OnDeviceChange);;
		}
	}
	
	[System.Serializable]
	public class IconSwitch
	{
		public string Name;

		[Header("In order: PC Icon, Gamepad Icon")]
		public Sprite[] DeviceIcons = new Sprite[2];

		[Header("Sprites to Switch")]
		public SpriteRenderer[] SwitchableIcons;
		[Header("UI Images to Switch")]
		public Image[] SwitchableUIImages;

		public ActiveObject[] ActiveObjects;
		
		[System.Serializable]
		public struct ActiveObject
		{
			public GameObject KeyboardObject;
			public GameObject GamepadObject;
		}
	}
}