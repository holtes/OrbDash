using System.Collections.Generic;
using System.Linq;

public class GameModel
{
    public int PlayersCount { get; private set; }
    
    public float MatchDuration { get; private set; }

    private Dictionary<int, PlayerState> _playersState = new();

    public GameModel(GameConfig gameConfig)
    {
        MatchDuration = gameConfig.MatchDurationSec;
    }

    public void SetPlayersCount(int count)
    {
        PlayersCount = count;
    }

    public void AddPlayerToSession(int id, string name)
    {
        _playersState[id] = new PlayerState
        {
            Id = id,
            Name = name,
            Score = 0
        };
    }

    public void AddScore(int playerId, int score = 1)
    {
        if (_playersState.TryGetValue(playerId, out var state))
            _playersState[playerId] = state.AddScore(score);
    }

    public string GetLeaderName() =>
        _playersState.OrderByDescending(p => p.Value.Score).First().Value.Name;

    public int GetLeaderScore() =>
        _playersState.OrderByDescending(p => p.Value.Score).First().Value.Score;
}
