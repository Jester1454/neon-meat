using System.Collections.Generic;
using System.Linq;
using DataPersistence;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
	public class SaveManager : MonoBehaviour
	{
		[SerializeField] private LevelCollection _levelCollection;

		private static SaveManager _instance = null;

		public static SaveManager Instance
		{
			get
			{
				if (!_instance)
				{
					var prefab = Resources.Load<GameObject>("SaveManager");
					var inScene = Instantiate<GameObject>(prefab);
					_instance = inScene.GetComponent<SaveManager>();
					DontDestroyOnLoad(_instance.transform.root.gameObject);
				}
				return _instance;
			}
		}
		
		public FileDataHandler _fileDataHandler;
		private SaveState _saveState;
		private const string ProfileName = "default";

		public SaveState SaveState => _saveState;
		public LevelCollection LevelCollection => _levelCollection;

		private void Awake()
		{
			Init();
		}
		
		private void Init()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);

			_fileDataHandler = new FileDataHandler(Application.persistentDataPath, ProfileName, false);
			TryLoad();
		}

		private void TryLoad()
		{
			if (_fileDataHandler.TryLoad<SaveState>(ProfileName, out var saveState))
			{
				_saveState = saveState;
				TryAddNewLevelToOldSaves();
			}
			else
			{
				CreateNewSaves();
			}
		}

		private void CreateNewSaves()
		{
			_saveState = new SaveState {LevelSaveDatas = new List<LevelSaveData>()};
			for (var i = 0; i < _levelCollection.Levels.Count; i++)
			{
				var level = _levelCollection.Levels[i];
				var levelSaveData = new LevelSaveData
				{
					SceneName = level.SceneName,
					BestResult = -1,
					State = i == 0 ? LevelState.Available : LevelState.Locked
				};

				_saveState.LevelSaveDatas.Add(levelSaveData);
			}

			_fileDataHandler.Save<SaveState>(_saveState, ProfileName);
		}

		private void TryAddNewLevelToOldSaves()
		{
			var hasChanged = false;
			for (int i = 0; i < _levelCollection.Levels.Count; i++)
			{
				var level = _levelCollection.Levels[i];
				if (_saveState.LevelSaveDatas.Any(x => x.SceneName.Equals(level.SceneName))) continue;
				
				hasChanged = true;
				_saveState.LevelSaveDatas.Add
				(
					new LevelSaveData
					{
						SceneName = level.SceneName,
						BestResult = -1,
						State = i == 0 ? LevelState.Available : LevelState.Locked
					}
				);
			}

			if (hasChanged)
			{
				_fileDataHandler.Save<SaveState>(_saveState, ProfileName);
			}
		}

		public double FinishLevel(double result)
		{
			var sceneName = SceneManager.GetActiveScene().name;
			double oldLevelResult = -1f;
			for (var i = 0; i < _saveState.LevelSaveDatas.Count; i++)
			{
				var levelSaveData = _saveState.LevelSaveDatas[i];
				if (levelSaveData.SceneName.Equals(sceneName))
				{
					levelSaveData.State = LevelState.Passed;
					oldLevelResult = levelSaveData.BestResult;
					levelSaveData.BestResult = result;
					if (i < _saveState.LevelSaveDatas.Count - 1)
					{
						var nextLevelSaveData = _saveState.LevelSaveDatas[i + 1];

						if (nextLevelSaveData.State == LevelState.Locked)
							nextLevelSaveData.State = LevelState.Available;
					}

					break;
				}
			}

			_fileDataHandler.Save(_saveState, ProfileName);

			return oldLevelResult;
		}

		public (LevelSaveData, LevelData) GetCurrentLevelData()
		{
			var sceneName = SceneManager.GetActiveScene().name;
			double oldLevelResult = -1f;
			for (var i = 0; i < _levelCollection.Levels.Count; i++)
			{
				var levelData = _levelCollection.Levels[i];
				if (levelData.SceneName.Equals(sceneName))
				{
					return (_saveState.LevelSaveDatas[i], levelData);
				}
			}

			return (new LevelSaveData {BestResult = -1}, new LevelData());
		}

		public void LoadFirsUnlockedLevel()
		{
			var sceneName = GetFirsUnlockedLevelSceneName();

			if (!string.IsNullOrEmpty(sceneName))
			{
				SceneManager.LoadScene(sceneName);
				return;
			}
			
			SceneManager.LoadScene(1);
		}

		public string GetFirsUnlockedLevelSceneName()
		{
			for (var i = 0; i < _saveState.LevelSaveDatas.Count; i++)
			{
				var levelSaveData = _saveState.LevelSaveDatas[i];

				if (levelSaveData.State == LevelState.Available)
				{
					return levelSaveData.SceneName;
				}
			}

			return string.Empty;
		}

		public bool LoadNextLevel()
		{
			var sceneName = SceneManager.GetActiveScene().name;
			for (var i = 0; i < _saveState.LevelSaveDatas.Count; i++)
			{
				var levelSaveData = _saveState.LevelSaveDatas[i];
				if (levelSaveData.SceneName.Equals(sceneName))
				{
					if (i < _saveState.LevelSaveDatas.Count - 1)
					{
						var nextLevelSaveData = _saveState.LevelSaveDatas[i + 1];
						SceneManager.LoadScene(nextLevelSaveData.SceneName);
						return true;
					}
				}
			}

			return false;
		}

		public bool AllLevelPassed()
		{
			var result = true;
			for (var i = 0; i < _saveState.LevelSaveDatas.Count; i++)
			{
				var levelSaveData = _saveState.LevelSaveDatas[i];

				if (levelSaveData.State != LevelState.Passed)
				{
					result = false;
					break;
				}
			}
			return result;
		}
	}
}