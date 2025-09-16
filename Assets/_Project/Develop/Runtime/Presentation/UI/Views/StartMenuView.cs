using UnityEngine;
using UnityEngine.UI;
using TMPro;
using R3;

public class StartMenuView : MonoBehaviour
{
    [SerializeField] private Slider _playersCountSlider;
    [SerializeField] private TMP_Text _playersCountTxt;
    [SerializeField] private Button _startGameBtn;

    public Observable<int> OnStartGameBtnClicked => _onStartGameBtnClicked;

    private Subject<int> _onStartGameBtnClicked = new();

    private void Awake()
    {
        _playersCountSlider
            .OnValueChangedAsObservable()
            .Subscribe(SetPlayersCount)
            .AddTo(this);


        _startGameBtn
            .OnClickAsObservable()
            .Subscribe(_ => StartBtnClick())
            .AddTo(this);
    }

    private void StartBtnClick()
    {
        _onStartGameBtnClicked.OnNext((int)_playersCountSlider.value);
    }

    private void SetPlayersCount(float value)
    {
        _playersCountTxt.text = value.ToString();
    }
}
