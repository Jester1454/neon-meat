using System;
using Core;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sound
{
	public class SelectLevelMenu : MonoBehaviour
	{
		[SerializeField] private Transform _parent;
		[SerializeField] private LevelItemView _levelItemView;
		[SerializeField] private Text _percent;
		[SerializeField] private Text _totalTime;

		private void Awake()
		{
			var saveState = SaveManager.Instance.SaveState;
			var levelCollection = SaveManager.Instance.LevelCollection;
			bool allLevelPassed = true;
			double totalTime = 0;
			float passedPercent = 0;
			var selectedScene = SaveManager.Instance.GetFirsUnlockedLevelSceneName();

			for (var i = 0; i < levelCollection.Levels.Count; i++)
			{
				var levelSaveData = saveState.LevelSaveDatas[i];
				var levelData = levelCollection.Levels[i];
				
				var levelItemView = Instantiate(_levelItemView, _parent);
				levelItemView.Init(levelSaveData, levelData);
				passedPercent += (int) levelSaveData.GetLevelResult(levelData) * 0.25f + 0.25f;
				totalTime += levelSaveData.BestResult;
				
				if (levelSaveData.SceneName.Equals(selectedScene))
				{
					EventSystem.current.SetSelectedGameObject(levelItemView.gameObject);
				}

				if (levelSaveData.State != LevelState.Passed)
				{
					allLevelPassed = false;
				}
			}

			if (string.IsNullOrEmpty(selectedScene))
			{
				EventSystem.current.SetSelectedGameObject(_parent.GetChild(_parent.childCount - 1).gameObject);
			}
			
			_percent.text = $"Percent: {Mathf.FloorToInt(passedPercent / levelCollection.Levels.Count * 100)}%";
			
			_totalTime.gameObject.SetActive(allLevelPassed);
			_totalTime.text = $"Total time: {TimeSpan.FromMilliseconds(totalTime).ToMyFormat()}";
		}
	}
}