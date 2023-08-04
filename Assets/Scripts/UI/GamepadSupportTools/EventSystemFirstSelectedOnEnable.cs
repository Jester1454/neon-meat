using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class EventSystemFirstSelectedOnEnable : MonoBehaviour
	{
		[SerializeField] private GameObject _firstSelected;

		private void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(_firstSelected);
		}
	}
}