using System;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Sound
{
	public class TimerView : MonoBehaviour
	{
		[SerializeField] private Text _timer;

		public void UpdateValue(TimeSpan value)
		{
			_timer.text = value.ToMyFormat();
		}
	}
}