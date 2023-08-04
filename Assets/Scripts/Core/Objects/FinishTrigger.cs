using System;
using Input;
using Sound;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;

namespace Core.Objects
{
	public class FinishTrigger : MonoBehaviour
	{
		private bool _isFinished;
		private bool _isStarted = false;
		
		private DateTime _startTime;
		private TimeSpan _timeElapsed;
		private NewInputDriver _inputDriver;
		private TimerView _timerView;
		
		private void Awake()
		{
			_inputDriver = FindObjectOfType<NewInputDriver>();
			_timerView = FindObjectOfType<TimerView>();
			InputSystem.onAnyButtonPress
				.CallOnce(ctrl => StartTime());

		}

		private void StartTime()
		{
			_startTime = DateTime.Now;
			_isStarted = true;
		}

		private void Update()
		{
			if (_isFinished) return;

			if (_isStarted)
			{
				_timeElapsed = DateTime.Now - _startTime;
			}

			_timerView.UpdateValue(_timeElapsed);
			if (_inputDriver.Rerorll)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
		
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (_isFinished) return;
			if (other == null) return;

			var player = other.GetComponent<Player>();

			if (player != null)
			{
				_isFinished = true;
				FindObjectOfType<FinishLevelMenu>().FinishLevel(_timeElapsed);
				Destroy(gameObject);
			}
		}
	}
}