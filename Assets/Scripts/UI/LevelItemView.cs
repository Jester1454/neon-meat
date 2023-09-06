using System;
using Core;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sound
{
	public class LevelItemView : MonoBehaviour
	{
		[SerializeField] private GameObject _lockObject;
		[SerializeField] private Text _name;
		[SerializeField] private Text _time;
		[SerializeField] private Button _button;
		[SerializeField] private Color _chapterTwoColor;
		[Space]
		[SerializeField] private GameObject _bronzeMedal;
		[SerializeField] private GameObject _silverMedal;
		[SerializeField] private GameObject _goldenMedal;
		private LevelSaveData _data;
		
		public void Init(LevelSaveData saveData, LevelData levelData, bool isChapterTwo)
		{
			_data = saveData;
			_lockObject.SetActive(saveData.State == LevelState.Locked || saveData.State == LevelState.None);
			_name.text = levelData.LevelName;

			_time.text = $"Best time \n{TimeSpan.FromMilliseconds(saveData.BestResult).ToMyFormat()}";
			_time.gameObject.SetActive(saveData.State == LevelState.Passed);
			_button.onClick.AddListener(Onclick);

			var resultType = saveData.GetLevelResult(levelData);
			_bronzeMedal.SetActive(resultType == LevelResultType.Bronze);
			_silverMedal.SetActive(resultType == LevelResultType.Silver);
			_goldenMedal.SetActive(resultType == LevelResultType.Golden);
			if (isChapterTwo)
			{
				var buttonColors = _button.colors;
				buttonColors.normalColor = _chapterTwoColor;
				_button.colors = buttonColors;
			} 
		}

		private void Onclick()
		{
			if (_data.State != LevelState.Available || _data.State != LevelState.Passed)
			{
				SceneManager.LoadScene(_data.SceneName);
			}
		}
	}
}