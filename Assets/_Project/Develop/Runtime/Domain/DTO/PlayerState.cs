public struct PlayerState
{
    public int Id;
    public string Name;
    public int Score;

    public PlayerState AddScore(int value)
    {
        Score += value;
        return this;
    }
}
