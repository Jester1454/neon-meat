using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

namespace LeaderBoards
{
    public class LeaderboardLoader
    {
        private readonly string _leaderboardId;
        
        public LeaderboardLoader(string leaderboardId)
        {
            _leaderboardId = leaderboardId;
        }
        
        public async Task<LeaderboardEntry> AddScore(double score)
        {
            var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(_leaderboardId, score);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
            return scoreResponse;
        }

        public async Task<LeaderboardEntry> GetPlayerScore()
        {
            var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(_leaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoreResponse));
            return scoreResponse;
        }
        
        public async Task<LeaderboardScoresPage> GetScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId);
            Debug.Log(JsonConvert.SerializeObject(scoresResponse));
            return scoresResponse;
        }
        
        public async Task<LeaderboardScoresPage> GetPaginatedScores(int limit, int offset)
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(_leaderboardId, new GetScoresOptions {Offset = offset, Limit = limit});
            return scoresResponse;
        }
    }
}