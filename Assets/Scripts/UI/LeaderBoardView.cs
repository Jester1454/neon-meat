using LeaderBoards;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace UI
{
	public class LeaderBoardView : MonoBehaviour
	{
		[SerializeField] private LeaderBoardEntryView _entryView;
		[SerializeField] private Transform _parent;
		[SerializeField] private GameObject _loader;
		
		public async void Show(LeaderboardLoader leaderBoardLoader, LeaderboardEntry player)
		{
			var scores = await leaderBoardLoader.GetScores();

			if (_loader != null)
			{
				_loader.SetActive(false);
			}
			else
			{
				return;
			}
			
			foreach (var result in scores.Results)
			{
				var entryView = Instantiate(_entryView, _parent);
				entryView.Init(result, result.PlayerId.Equals(AuthenticationService.Instance.PlayerId));
			}
		}
	}
}