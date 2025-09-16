using System;
using UnityEngine;
using Zenject;
using R3;

public class OrbsManager : MonoBehaviour
{
    private IDisposable _captureStream;
    private GameObject _spawnedOrb;

    private OrbsManagerModel _model;
    private Transform _sceneOrigin;
    private GameObject _orbPrefab;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        OrbsManagerModel orbsManagerModel,
        [Inject(Id = "Scene")] Transform sceneOrigin,
        GameObject orbPrefab,
        SignalBus signalBus
    )
    {
        _model = orbsManagerModel;
        _sceneOrigin = sceneOrigin;
        _orbPrefab = orbPrefab;
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<OnOrbCapturedSignal>(CaptureOrb);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<OnOrbCapturedSignal>(CaptureOrb);
    }

    public void SpawnOrb() 
    {
        var orbPosition = _model.GetRandomPointInArena();
        _spawnedOrb = Instantiate(_orbPrefab, orbPosition, Quaternion.identity, _sceneOrigin);
    }

    public void DestroyOrbs()
    {
        _captureStream?.Dispose();
        if (_spawnedOrb) Destroy(_spawnedOrb);
    }

    private void CaptureOrb(OnOrbCapturedSignal signal)
    {
        _captureStream?.Dispose();

        _captureStream = Observable
            .Timer(TimeSpan.FromSeconds(_model.OrbCaptureTime))
            .Subscribe(_ => CaptureComplete(signal.PlayerId))
            .AddTo(this);
    }

    private void CaptureComplete(int playerId)
    {
        _spawnedOrb = null;
        _signalBus.Fire(new OnScoreAddedSignal(playerId));
        Observable
            .Timer(TimeSpan.FromSeconds(_model.OrbRespawnDelay))
            .Subscribe(_ => SpawnOrb())
            .AddTo(this);
    }
}
