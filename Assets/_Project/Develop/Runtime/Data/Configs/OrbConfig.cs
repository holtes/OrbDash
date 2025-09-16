using UnityEngine;

[CreateAssetMenu(fileName = "OrbConfig", menuName = "Configs/OrbConfig")]
public class OrbConfig : ScriptableObject
{
    public float CaptureTime = 5f;
    public float RespawnDelay = 2f;
}
