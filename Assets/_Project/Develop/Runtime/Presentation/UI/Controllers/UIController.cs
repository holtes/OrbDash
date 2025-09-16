using UnityEngine;
using Zenject;
using R3;
using TSS;

public class UIController : MonoBehaviour
{
    [SerializeField] private TSSCore _tssCore;
    [SerializeField] private StartMenuView _startMenuView;
    [SerializeField] private EndGameView _endGameView;

    private SignalBus _signalBus;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Awake()
    {
        _signalBus.Subscribe<OnMatchEndSignal>(EndGame);

        _startMenuView
            .OnStartGameBtnClicked
            .Subscribe(StartGame)
            .AddTo(this);

        _endGameView
            .OnGoToMenuBtnClicked
            .Subscribe(_ => OpenStartMenu())
            .AddTo(this);

        _endGameView
            .OnRestartBtnClicked
            .Subscribe(_ => RestartGame())
            .AddTo(this);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<OnMatchEndSignal>(EndGame);
    }

    private void StartGame(int playersCount)
    {
        CloseWindows();
        _signalBus.Fire(new OnMatchStartSignal(playersCount));
    }

    private void RestartGame()
    {
        CloseWindows();
        _signalBus.Fire(new OnMatchRestartSignal());
    }

    private void EndGame(OnMatchEndSignal signal)
    {
        OpenEndGameMenu();
        _endGameView.SetPlayerName(signal.LeaderName);
    }

    private void CloseWindows()
    {
        _tssCore.CloseAll();
    }

    public void OpenStartMenu()
    {
        _tssCore.SelectState("StartMenu");
    }

    public void OpenEndGameMenu()
    {
        _tssCore.SelectState("EndGameMenu");
    }
}
