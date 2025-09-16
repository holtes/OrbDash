using UnityEngine;

public class OrbsManagerModel
{
    private readonly Bounds _arenaBounds;

    public float OrbCaptureTime { get; private set; }
    public float OrbRespawnDelay { get; private set; }

    public OrbsManagerModel(OrbConfig orbConfig, Transform arenaOrigin)
    {
        OrbCaptureTime = orbConfig.CaptureTime;
        OrbRespawnDelay = orbConfig.RespawnDelay;
        _arenaBounds = arenaOrigin.GetComponent<BoxCollider2D>().bounds;
    }

    public Vector2 GetRandomPointInArena()
    {
        return new Vector2(
            Random.Range(_arenaBounds.min.x, _arenaBounds.max.x),
            Random.Range(_arenaBounds.min.y, _arenaBounds.max.y)
        );
    }
}
