using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private string _sceneName;
        
        private void Awake()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}

