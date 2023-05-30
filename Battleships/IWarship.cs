namespace Battleships;

public interface IWarship
{
    Coordinate Start { get; }
    Direction Direction { get; }
    int Length { get; }
    bool CheckPosition(Coordinate coordinates);
    IReadOnlyList<Coordinate> Coordinates { get; }
    int HitCount { get; }
    public bool Sinks { get; }
    void Hit();
}

[Flags]
public enum Direction
{
    South = 0,
    East = 1
}