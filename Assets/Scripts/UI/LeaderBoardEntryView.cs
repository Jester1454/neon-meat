using System;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class LeaderBoardEntryView : MonoBehaviour
	{
		[SerializeField] private Text _nickName;
		[SerializeField] private Text _place;
		[SerializeField] private Text _time;
		[SerializeField] private GameObject _isMe;
		
		public void Init(LeaderboardEntry entry, bool isMe)
		{
			var nickName = entry.PlayerName;			
			var index = nickName.IndexOf("#", StringComparison.Ordinal);
			if (index >= 0)
				nickName = nickName.Substring(0, index);
			
			_nickName.text = nickName;
			_place.text = (entry.Rank + 1).ToString();
			_time.text = TimeSpan.FromMilliseconds(entry.Score).ToMyFormat();
			_isMe.SetActive(isMe);
		}
	}
}