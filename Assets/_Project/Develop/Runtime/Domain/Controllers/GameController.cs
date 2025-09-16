using System;
using UnityEngine;
using Zenject;
using R3;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayersJoinController _playerJoinController;
    [SerializeField] private OrbsManager _orbsManager;

    private IDisposable _matchStream;

    private GameModel _model;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(GameModel gameModel, SignalBus signalBus)
    {
        _model = gameModel;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<OnMatchStartSignal>(StartGame);
        _signalBus.Subscribe<OnMatchRestartSignal>(RestartGame);
        _signalBus.Subscribe<OnScoreAddedSignal>(AddScore);
        _signalBus.Subscribe<OnPlayerJoinedSignal>(JoinPlayer);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<OnMatchStartSignal>(StartGame);
        _signalBus.Unsubscribe<OnMatchRestartSignal>(RestartGame);
        _signalBus.Unsubscribe<OnScoreAddedSignal>(AddScore);
        _signalBus.Unsubscribe<OnPlayerJoinedSignal>(JoinPlayer);
    }

    private void StartGame(OnMatchStartSignal signal)
    {
        _model.SetPlayersCount(signal.PlayersCount);

        SetUpGame();
    }

    private void RestartGame()
    {
        SetUpGame();
    }

    private void SetUpGame()
    {
        _matchStream?.Dispose();

        for (int i = 0; i < _model.PlayersCount; i++)
        {
            _playerJoinController.JoinPlayer(i);
        }

        _orbsManager.SpawnOrb();

        _matchStream = Observable
            .Timer(TimeSpan.FromSeconds(_model.MatchDuration))
            .Subscribe(_ => EndGame())
            .AddTo(this);
    }

    private void EndGame()
    {
        _playerJoinController.LeftPlayers();
        _orbsManager.DestroyOrbs();
        _signalBus.Fire(new OnMatchEndSignal(_model.GetLeaderName()));
    }

    private void JoinPlayer(OnPlayerJoinedSignal signal)
    {
        _model.AddPlayerToSession(signal.Id, signal.Name);
    }

    private void AddScore(OnScoreAddedSignal signal)
    {
        _model.AddScore(signal.PlayerId);
    }
}
