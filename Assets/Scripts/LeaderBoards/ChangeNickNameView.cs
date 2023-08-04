using Core;
using UI;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace LeaderBoards.LeaderBoards
{
	public class ChangeNickNameView : MonoBehaviour
	{
		[SerializeField] private InputField _inputField;
		[SerializeField] private Button _acceptButton;
		
		private void Awake()
		{
			_acceptButton.onClick.AddListener(OnAcceptClick);
		}

		private void OnAcceptClick()
		{
			if (string.IsNullOrEmpty(_inputField.text))
			{
				SaveManager.Instance.LoadNextLevel();
				return;
			}

			AuthenticationService.Instance.UpdatePlayerNameAsync(_inputField.text);
			
			PlayerPrefs.SetInt(StartGameButton.FirstPlayKey, 1);
			PlayerPrefs.Save();
			
			SaveManager.Instance.LoadFirsUnlockedLevel();
		}
	}
}