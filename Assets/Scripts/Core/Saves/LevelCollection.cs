using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
	[CreateAssetMenu]
	public class LevelCollection : ScriptableObject
	{
		public List<LevelData> Levels;
	}

	[Serializable]
	public struct LevelData
	{
		public string SceneName;
		[FormerlySerializedAs("LeveName")] public string LevelName;
		[FormerlySerializedAs("BestDeveloperResult")] public double GoldResult;
		public double SilverResult;
		public double BronzeResult;
	}
}