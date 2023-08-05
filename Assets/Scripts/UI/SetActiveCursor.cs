using UnityEngine;

namespace Sound
{
	public class SetActiveCursor : MonoBehaviour
	{
		[SerializeField] private bool _showOnEnable;
		[SerializeField] private bool _hideOnDisable;


		private void OnEnable()
		{
			if (_showOnEnable)
			{
				Cursor.visible = true;
			}
		}

		private void OnDisable()
		{
			if (_hideOnDisable)
			{
				Cursor.visible = false;
			}
		}
	}
}