using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersJoinModel
{
    private readonly List<int> _joinedPlayers = new();
    private readonly Dictionary<int, PlayerSpawnData> _playerSpawnDatas = new();

    public float PlayersSpeed { get; private set; }
    public List<Vector3> PlayersSpawnPoints { get; private set; }

    public PlayersJoinModel(GameConfig gameConfig, List<Transform> playersSpawnPoints)
    {
        PlayersSpeed = gameConfig.PlayerSpeed;
        PlayersSpawnPoints = playersSpawnPoints.Select(el => el.position).ToList();
        _playerSpawnDatas = gameConfig.Players
            .Select((player, index) => new PlayerSpawnData
            {
                Id = index,
                Position = PlayersSpawnPoints[index],
                Color = player.Color,
                Speed = gameConfig.PlayerSpeed,
                Acceleration = gameConfig.Acceleration,
                InputMap = player.InputMap
            })
            .ToDictionary(p => p.Id);
    }

    public bool GetJoinedPlayer(int id, out PlayerSpawnData joinedPlayer)
    {
        joinedPlayer = default;
        if (_joinedPlayers.Contains(id))
        {
            if (_playerSpawnDatas.TryGetValue(id, out joinedPlayer)) return true;
            return false;
        }
        return false;
    }

    public string JoinPlayer(int id)
    {
        _joinedPlayers.Add(id);

        if (_playerSpawnDatas.TryGetValue(id, out var spawnData)) return spawnData.InputMap;
        return string.Empty;
    }

    public void LeftPlayer(int id)
    {
        _joinedPlayers.Remove(id);
    }
}
