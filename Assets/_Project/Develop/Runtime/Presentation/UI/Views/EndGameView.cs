using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;

public class EndGameView : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameTxt;
    [SerializeField] private Button _goToMenuBtn;
    [SerializeField] private Button _restartBtn;

    public Observable<Unit> OnGoToMenuBtnClicked => _onGoToMenuBtnClicked;
    public Observable<Unit> OnRestartBtnClicked => _onRestartBtnClicked;

    private Subject<Unit> _onGoToMenuBtnClicked = new();
    private Subject<Unit> _onRestartBtnClicked = new();

    private void Awake()
    {
        _goToMenuBtn
            .OnClickAsObservable()
            .Subscribe(_onGoToMenuBtnClicked.AsObserver())
            .AddTo(this);

        _restartBtn
            .OnClickAsObservable()
            .Subscribe(_onRestartBtnClicked.AsObserver())
            .AddTo(this);
    }

    public void SetPlayerName<T>(T name)
    {
        _playerNameTxt.text = name.ToString();
    }
}
