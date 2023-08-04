using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] private GameObject _windowParent;
		[SerializeField] private Button _exitButton;
		[SerializeField] private GameObject _firstSelected;
		
		private bool _isPause = false;
		private bool _isDisabled = false;

		public void DisabledSaveMenu()
		{
			_isDisabled = true;
		}
		
		private void Start()
		{
			FindObjectOfType<NewInputDriver>().PlayerControls.Gameplay.Pause.started += context => SetActivePause(!_isPause);
			_exitButton.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			SetActivePause(false);
		}

		private void SetActivePause(bool value)
		{
			if (_isDisabled) return;
			 
			_isPause = value;
			_windowParent.SetActive(value);
			Time.timeScale = value ? 0 : 1f;
			if (_isPause)
			{
				EventSystem.current.SetSelectedGameObject(_firstSelected);
			}
		}

		private void OnDestroy()
		{
			Time.timeScale = 1f;
		}
	}
}