using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private OrbConfig _orbConfig;
    [SerializeField] private GameObject _orbPrefab;
    [SerializeField] private Transform _sceneOrigin;
    [SerializeField] private Transform _arenaOrigin;
    [SerializeField] private List<Transform> _playersSpawnPoints;

    public override void InstallBindings()
    {
        BindSignalBus();
        BindSignals();
        BindOrbPrefab();
        BindSceneOrigin();
        BindArenaOrigin();
        BindGameModel();
        BindPlayersJoinModel();
        BindOrbsManagerModel();
    }

    private void BindSignalBus()
    {
        SignalBusInstaller.Install(Container);
    }

    private void BindSignals()
    {
        Container.DeclareSignal<OnMatchStartSignal>();
        Container.DeclareSignal<OnMatchEndSignal>();
        Container.DeclareSignal<OnMatchRestartSignal>();
        Container.DeclareSignal<OnPlayerJoinedSignal>();
        Container.DeclareSignal<OnOrbCapturedSignal>();
        Container.DeclareSignal<OnScoreAddedSignal>();
        
    }

    private void BindOrbPrefab()
    {
        Container
            .Bind<GameObject>()
            .FromInstance(_orbPrefab);
    }

    private void BindSceneOrigin()
    {
        Container
            .Bind<Transform>()
            .WithId("Scene")
            .FromInstance(_sceneOrigin);
    }

    private void BindArenaOrigin()
    {
        Container
            .Bind<Transform>()
            .WithId("Arena")
            .FromInstance(_arenaOrigin);
    }

    private void BindGameModel()
    {
        Container
            .Bind<GameModel>()
            .AsSingle()
            .WithArguments(_gameConfig);
    }

    private void BindPlayersJoinModel()
    {
        Container
            .Bind<PlayersJoinModel>()
            .AsSingle()
            .WithArguments(_gameConfig, _playersSpawnPoints);
    }

    private void BindOrbsManagerModel()
    {
        Container
            .Bind<OrbsManagerModel>()
            .AsSingle()
            .WithArguments(_orbConfig, _arenaOrigin);
    }
}