using UnityEngine;

namespace PlayerControllers
{
	public class DirectionView : MonoBehaviour
	{
		[SerializeField] private Color _availableColor;
		[SerializeField] private Color _noAvailableColor;
		[SerializeField] private SpriteRenderer _view;
		
		private bool _isAvailable = true;
		
		public void SetState(bool value)
		{
			if (_isAvailable == value) return;

			_isAvailable = value;
			_view.color = _isAvailable ? _availableColor : _noAvailableColor;
		}
	}
}