using UnityEngine;
using UnityEngine.Events;

namespace PlayerControllers
{
	public class SmolSwitch : MonoBehaviour, ISwitch
	{
		[SerializeField] private AudioClip _switchClip;
		
		public UnityEvent OnSwitch;
		private bool _isSwitch = false;
		
		public void Switch()
		{
			if (_isSwitch) return;
			_isSwitch = true;
			OnSwitch?.Invoke();
			Destroy(gameObject);
		}
	}
}