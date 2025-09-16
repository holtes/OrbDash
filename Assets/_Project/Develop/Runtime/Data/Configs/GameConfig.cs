using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    public float MatchDurationSec = 90f;
    public float PlayerSpeed = 6f;
    public float Acceleration = 10f;
    public List<Player> Players = new List<Player>();
}
