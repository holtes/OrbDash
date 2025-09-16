using UnityEngine;
using TMPro;
using R3;
using R3.Triggers;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerView : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private GameObject _orbMark;
    [SerializeField] private GameObject _sprintTrail;

    public Observable<Unit> OnOrbCaptured => _onOrbCaptured;
    public Observable<Unit> OnOrbStealed => _onOrbStealed;

    private Subject<Unit> _onOrbCaptured = new();
    private Subject<Unit> _onOrbStealed = new();

    private Vector2 _velocity;

    private void Awake()
    {
        this
            .FixedUpdateAsObservable()
            .Subscribe(_ => MovePlayer())
            .AddTo(this);

        this
            .OnTriggerEnter2DAsObservable()
            .Subscribe(DetectOrb)
            .AddTo(this);

        this
            .OnCollisionEnter2DAsObservable()
            .Subscribe(DetectPlayer)
            .AddTo(this);
    }

    public void SetPlayerName(string name)
    {
        _playerName.text = name;
    }

    public void SetPlayerColor(Color color)
    {
        _playerRenderer.color = color;
    }

    private void DetectOrb(Collider2D collider)
    {
        if (collider.CompareTag("Orb"))
        {
            Destroy(collider.gameObject);
            _onOrbCaptured.OnNext(Unit.Default);
        }
            
    }

    private void DetectPlayer(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<IPlayerController>(out var player))
        {
            if (player.HasOrb()) _onOrbStealed.OnNext(Unit.Default);
        }
    }

    public void SetPlayerVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    private void MovePlayer()
    {
        _rb.linearVelocity = _velocity;
        if (_velocity.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(-_velocity.y, -_velocity.x) * Mathf.Rad2Deg;

            targetAngle += -90f;

            float smoothAngle = Mathf.LerpAngle(
                _sprintTrail.transform.eulerAngles.z,
                targetAngle,
                Time.fixedDeltaTime * 10f // скорость поворота, можно вынести в настройку
            );

            _sprintTrail.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
        }
    }

    public void EnableSprint()
    {
        _sprintTrail.SetActive(true);
    }

    public void DisableSprint()
    {
        _sprintTrail.SetActive(false);
    }

    public void EnableOrbMark()
    {
        _orbMark.SetActive(true);
    }

    public void DisableOrbMark()
    {
        _orbMark.SetActive(false);
    }
}

