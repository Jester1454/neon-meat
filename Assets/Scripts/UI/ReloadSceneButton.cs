using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class ReloadSceneButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
        
		private void Awake()
		{
			_button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}