using UnityEngine;

public class PlayerModel
{
    public int Id { get; }
    public string Name { get; }
    public Color Color { get; }
    public bool HasOrb { get; private set; }
    public bool IsSprinting => _isSprinting;
    public bool CanSprint => _canSprint && !_isSprinting;

    private float _speed;
    private float _acceleration;
    private bool _isSprinting;
    private bool _canSprint = true;

    public PlayerModel(int id, float speed, float acceleration, string name, Color color)
    {
        Id = id;
        Name = name;
        Color = color;
        _speed = speed;
        _acceleration = acceleration;
    }

    public float GetPlayerSpeed() =>
        _isSprinting ? _speed * _acceleration : _speed;

    public void CaptureOrb()
    {
        HasOrb = true;
    }

    public void LooseOrb()
    {
        HasOrb = false;
    }

    public void ActivateSprint()
    {
        if (!CanSprint) return;
        _isSprinting = true;
        _canSprint = false;
    }

    public void DeactivateSprint()
    {
        _isSprinting = false;
    }

    public void ResetSprintCooldown()
    {
        _canSprint = true;
    }
}
