using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
	public class EventSystemSetSelectButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _gameObject;
        
		private void Awake()
		{
			_button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			EventSystem.current.SetSelectedGameObject(_gameObject);
		}
	}
}