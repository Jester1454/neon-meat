using System;

namespace Core
{
	[Serializable]
	public class LevelSaveData
	{
		public string SceneName;
		public double BestResult = -1;
		public LevelState State;

		public LevelResultType GetLevelResult(LevelData levelData)
		{
			if (State != LevelState.Passed) return LevelResultType.None;

			if (BestResult < 0) return LevelResultType.None;
			
			if (BestResult <= levelData.GoldResult)
				return LevelResultType.Golden;			
			
			if (BestResult <= levelData.SilverResult)
				return LevelResultType.Silver;	
			
			if (BestResult <= levelData.BronzeResult)
				return LevelResultType.Bronze;

			return LevelResultType.OnlyPassed;
		}
	}

	public enum LevelResultType
	{
		None = -1,
		OnlyPassed = 0,
		Bronze = 1,
		Silver = 2,
		Golden = 3
	}
}