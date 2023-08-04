using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class SetActiveObjectButton : MonoBehaviour
	{
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _object;
		[SerializeField] private bool _value;
		
		private void Awake()
		{
			_button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			_object.SetActive(_value);
		}
	}
}