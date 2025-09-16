using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using R3;

public class PlayerController : MonoBehaviour, IPlayerController
{
    [SerializeField] private PlayerView _view;
    [SerializeField] private PlayerInputHandler _input;

    private PlayerModel _model;
    
    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<OnOrbCapturedSignal>(LooseOrb);
        _signalBus.Subscribe<OnScoreAddedSignal>(LooseOrb);

        _input
            .OnMoveInput
            .Subscribe(MovePlayer)
            .AddTo(this);

        _input
            .OnSprintInput
            .Subscribe(_ => ActivateSprint().Forget())
            .AddTo(this);

        _view
            .OnOrbCaptured
            .Subscribe(_ => CaptureOrb())
            .AddTo(this);

        _view
            .OnOrbStealed
            .Subscribe(_ => StealOrb())
            .AddTo(this);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<OnOrbCapturedSignal>(LooseOrb);
        _signalBus.Unsubscribe<OnScoreAddedSignal>(LooseOrb);
    }

    public void Init(PlayerModel playerModel)
    {
        _model = playerModel;
        _view.SetPlayerColor(_model.Color);
        _view.SetPlayerName(_model.Name);
    }

    private void CaptureOrb()
    {
        _model.CaptureOrb();
        _view.EnableOrbMark();
        _signalBus.Fire(new OnOrbCapturedSignal(_model.Id));
    }

    private void StealOrb()
    {
        if (_model.IsSprinting)
            CaptureOrb();
    }

    private void LooseOrb(OnOrbCapturedSignal signal)
    {
        if (_model.Id != signal.PlayerId)
        {
            _model.LooseOrb();
            _view.DisableOrbMark();
        }
    }

    private void LooseOrb(OnScoreAddedSignal signal)
    {
        if (_model.Id == signal.PlayerId)
        {
            _model.LooseOrb();
            _view.DisableOrbMark();
        }
    }

    private void MovePlayer(Vector2 moveInput)
    {
        _view.SetPlayerVelocity(moveInput * _model.GetPlayerSpeed());
    }

    private async UniTaskVoid ActivateSprint()
    {
        if (!_model.CanSprint) return;

        _model.ActivateSprint();
        _view.EnableSprint();

        await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: this.GetCancellationTokenOnDestroy());

        _model.DeactivateSprint();
        _view.DisableSprint();

        await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: this.GetCancellationTokenOnDestroy());

        _model.ResetSprintCooldown();
    }

    public bool HasOrb() => _model.HasOrb;
}
