using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace UI
{
	public class EventSystemLoadSceneOnCancel : MonoBehaviour
	{
		[SerializeField] private string _sceneName;
		private InputSystemUIInputModule _inputModule;
		
		private void OnEnable()
		{
			_inputModule = FindObjectOfType<InputSystemUIInputModule>();
			_inputModule.cancel.action.performed += LoadScene;
		}

		private void LoadScene(InputAction.CallbackContext context)
		{
			SceneManager.LoadScene(_sceneName);
		}
		
		private void OnDisable()
		{
			_inputModule.cancel.action.performed -= LoadScene;
		}
	}
}