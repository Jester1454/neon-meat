using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class StartGameButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _enterNickNamePopUp;
		
        public const string FirstPlayKey = "FirstPlay";
	        
		private void Awake()
		{
			_button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			var isFirstPlay = PlayerPrefs.GetInt(FirstPlayKey, 0);
			
			if (isFirstPlay == 0)
			{
				_enterNickNamePopUp.SetActive(true);
				return;
			}
			
			SaveManager.Instance.LoadFirsUnlockedLevel();
		}
	}
}