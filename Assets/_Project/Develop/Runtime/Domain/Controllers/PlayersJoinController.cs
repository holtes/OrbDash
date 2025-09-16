using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayersJoinController : MonoBehaviour
{
    [SerializeField] private PlayerInputManager _inputManager;

    private DiContainer _container;
    private PlayersJoinModel _model;
    private Transform _sceneOrigin;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        DiContainer container,
        PlayersJoinModel playersJoinModel,
        [Inject(Id = "Scene")] Transform sceneOrigin,
        SignalBus signalBus)
    {
        _container = container;
        _model = playersJoinModel;
        _sceneOrigin = sceneOrigin;
        _signalBus = signalBus;
    }

    public void JoinPlayer(int id)
    {
        _model.JoinPlayer(id);

        if (!_model.GetJoinedPlayer(id, out var playerData)) return;

        _inputManager.JoinPlayer(
            playerIndex: id,
            controlScheme: playerData.InputMap,
            pairWithDevice: Keyboard.current
        );
    }

    public void LeftPlayers()
    {
        var players = PlayerInput.all.ToArray();

        foreach (var player in players)
        {
            _model.LeftPlayer(player.playerIndex);

            Destroy(player.gameObject);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        var playerId = playerInput.playerIndex;

        if (!_model.GetJoinedPlayer(playerId, out var playerData)) return;

        var playerObj = playerInput.gameObject;

        playerObj.name = $"Player_{playerId}";
        playerObj.transform.parent = _sceneOrigin;
        playerObj.transform.position = playerData.Position;

        _container.InjectGameObject(playerObj);

        var playerModel = new PlayerModel(playerId, playerData.Speed, playerData.Acceleration,
            playerObj.name, playerData.Color);
        var playerController = playerObj.GetComponent<PlayerController>();

        playerController.Init(playerModel);

        _signalBus.Fire(new OnPlayerJoinedSignal(playerId, playerObj.name));
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        _model.LeftPlayer(playerInput.playerIndex);

        Destroy(playerInput.gameObject);
    }
}
