using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LoadNextLevelButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
        
		private void Awake()
		{
			_button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			SaveManager.Instance.LoadFirsUnlockedLevel();
		}
	}
}