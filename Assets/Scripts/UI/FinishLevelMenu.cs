using System;
using System.Collections;
using Core;
using Input;
using LeaderBoards;
using Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
	public class FinishLevelMenu : MonoBehaviour
	{
		[SerializeField] private Text _levelName;
		[SerializeField] private GameObject _windowParent;
		[SerializeField] private Button _nextLevelButton;
		[SerializeField] private GameObject _newRecord;
		[SerializeField] private Text _currentTime;
		[SerializeField] private Text _bestTime;
		[SerializeField] private AudioClip _winClip;
		[SerializeField] private AudioClip _finishJingle;
		[SerializeField] private GameObject _finishScreen;
		[SerializeField] private Text _totalTime;
		[SerializeField] private GameObject _firstSelected;
		[SerializeField] private LeaderBoardView _leaderBoardView;
		
		[Header("Reward table")]
		[SerializeField] private Text _bronzeResult;
		[SerializeField] private GameObject _bronzeIsPassed;
		[SerializeField] private Text _silverResult;
		[SerializeField] private GameObject _silverIsPassed;
		[SerializeField] private Text _goldResult;
		[SerializeField] private GameObject _goldIsPassed;
		[SerializeField] private GameObject _bronzeMedal;
		[SerializeField] private GameObject _silverMedal;
		[SerializeField] private GameObject _goldenMedal;
		
		private bool _levelIsFinished;
		
		private void Awake()
		{
			_nextLevelButton.onClick.AddListener(ShowFinalWindow);
		}

		public void FinishLevel(TimeSpan newResult)
		{
			EventSystem.current.SetSelectedGameObject(_firstSelected);
			SoundManager.Instance.Play(_winClip);
			
			StartCoroutine(WaitToSkip());
			_levelIsFinished = true;
			FindObjectOfType<PauseMenu>().DisabledSaveMenu();
			
			_windowParent.SetActive(true);

			ShowFinishResult(newResult);
		}

		private async void ShowFinishResult(TimeSpan newResult)
		{
			var levelData = SaveManager.Instance.GetCurrentLevelData();
			var oldResult = levelData.Item1.BestResult;

			var isRecordResult = oldResult < 0 || newResult.TotalMilliseconds < oldResult;
			SaveManager.Instance.FinishLevel(isRecordResult ? newResult.TotalMilliseconds : oldResult);

			_newRecord.gameObject.SetActive(isRecordResult);
			_levelName.text = levelData.Item2.LevelName.ToUpper();
			_currentTime.text = newResult.ToMyFormat();

			FillRewardResult(levelData.Item2, isRecordResult ? newResult.TotalMilliseconds : oldResult);
			
			var leaderBoardLoader = new LeaderboardLoader(levelData.Item2.SceneName);
			var playerEntry = isRecordResult
				? await leaderBoardLoader.AddScore(newResult.TotalMilliseconds)
				: await leaderBoardLoader.GetPlayerScore();
			_leaderBoardView.Show(leaderBoardLoader, playerEntry);
		}

		private void FillRewardResult(LevelData levelData, double recordResult)
		{
			_bronzeResult.text = TimeSpan.FromMilliseconds(levelData.BronzeResult).ToMyFormat();
			_silverResult.text = TimeSpan.FromMilliseconds(levelData.SilverResult).ToMyFormat();
			_goldResult.text = TimeSpan.FromMilliseconds(levelData.GoldResult).ToMyFormat();
			
			_bronzeIsPassed.SetActive(recordResult <= levelData.BronzeResult);
			_silverIsPassed.SetActive(recordResult <= levelData.SilverResult);
			_goldIsPassed.SetActive(recordResult <= levelData.GoldResult);
			
			_bronzeMedal.SetActive(recordResult <= levelData.BronzeResult);
			_silverMedal.SetActive(recordResult <= levelData.SilverResult);
			_goldenMedal.SetActive(recordResult <= levelData.GoldResult);
			_bestTime.text = TimeSpan.FromMilliseconds(recordResult).ToMyFormat();
		}

		private void ShowFinalWindow()
		{
			if (SaveManager.Instance.LoadNextLevel())
			{
				return;
			}
			if (PlayerPrefs.GetInt("finish", 0) == 1)
			{
				SceneManager.LoadScene(1);
				return;
			}
			if (!SaveManager.Instance.AllLevelPassed())
			{
				SceneManager.LoadScene(1);
				return;
			}
			
			SoundManager.Instance.Play(_finishJingle);
			_finishScreen.SetActive(true);
			_windowParent.SetActive(false);
			var saveState = SaveManager.Instance.SaveState;
			double totalTime = 0;
			foreach (var level in saveState.LevelSaveDatas)
			{
				totalTime += level.BestResult;
			}
				
			_totalTime.text = $"Total time: {TimeSpan.FromMilliseconds(totalTime).ToMyFormat()}";
			PlayerPrefs.SetInt("finish", 1);
		}		

		private IEnumerator WaitToSkip()
		{
			yield return new WaitForSeconds(0.2f);
			
			var inputDriver = FindObjectOfType<NewInputDriver>();

			inputDriver.PlayerControls.Gameplay.Jump.performed += (x) =>
			{
				if (_levelIsFinished)
				{
					ShowFinalWindow();
				}
			};
			
			inputDriver.PlayerControls.Gameplay.Reroll.performed += (x) =>
			{
				if (_levelIsFinished)
				{
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			};
		}
	}
}