using UnityEngine;
using UnityEngine.InputSystem;
using R3;

public class PlayerInputHandler : MonoBehaviour
{
    public Observable<Vector2> OnMoveInput => _onMoveInput;
    public Observable<Unit> OnSprintInput => _onSprintInput;

    private Subject<Vector2> _onMoveInput = new();
    private Subject<Unit> _onSprintInput = new();

    public void OnMove(InputValue value)
    {
        _onMoveInput.OnNext(value.Get<Vector2>());
    }

    public void OnSprint()
    {
        _onSprintInput.OnNext(Unit.Default);
    }
}
