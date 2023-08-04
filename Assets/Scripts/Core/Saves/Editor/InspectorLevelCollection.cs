using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.Inspector
{
	[CustomEditor(typeof(LevelCollection))]
	public class InspectorLevelCollection : Editor
	{
		private readonly List<string> _blackListScenes = new List<string> {"MainMenu", "LevelsScreen"};
		
		private LevelCollection _levelCollection;
		void OnEnable()
		{
			_levelCollection = (LevelCollection) target;
		}
 
		public override void OnInspectorGUI()
		{
			
			DrawDefaultInspector();

			if (GUILayout.Button("Update level data"))
			{
				UpdateLevelData();
			}
			if (GUI.changed)
				EditorUtility.SetDirty(_levelCollection);
		}

		private void UpdateLevelData()
		{
			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if (!scene.enabled) continue;
				var sceneName = GetSceneNameFromScenePath(scene.path);
				if (string.IsNullOrEmpty(sceneName)) continue;
				
				if (!_blackListScenes.Contains(sceneName) && _levelCollection.Levels.Count(x => x.SceneName.Equals(sceneName)) == 0)
				{
					_levelCollection.Levels.Add(new LevelData
					{
						SceneName = sceneName
					});
				}
			}
		}

		private static string GetSceneNameFromScenePath(string scenePath)
		{
			// Unity's asset paths always use '/' as a path separator
			var sceneNameStart = scenePath.LastIndexOf("/", StringComparison.Ordinal) + 1;
			var sceneNameEnd = scenePath.LastIndexOf(".", StringComparison.Ordinal);
			var sceneNameLength = sceneNameEnd - sceneNameStart;
			return scenePath.Substring(sceneNameStart, sceneNameLength);
		}
	}
}