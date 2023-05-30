namespace Battleships;

public abstract class Warship : IWarship
{
    public Coordinate Start { get; }
    public Direction Direction { get; }
    public int Length { get; }
    public int HitCount { get; private set; }
    public bool Sinks => HitCount == Length;
    private List<Coordinate> _coordinates { get; } = new List<Coordinate>();
    
    private int[] Position { get; }
    public IReadOnlyList<Coordinate> Coordinates => _coordinates;

    protected Warship(Coordinate start, Direction direction, int length)
    {
        Start = start;
        Direction = direction;
        Length = length;

        Position = new int[length];
        
        _coordinates.Add(start);

        if (direction == Direction.East)
        {
            int column = Start.Column;

            for (int i = 1; i < Length; i++)
            {
                column++;
                _coordinates.Add(Coordinate.Create((char)column, Start.Row));
            }
        }

        if (direction == Direction.South)
        {
            int row = Start.Row;
            for (int i = 1; i < Length; i++)
                _coordinates.Add(Coordinate.Create(start.Column, ++row));
        }
    }

    public bool CheckPosition(Coordinate coordinates)
    {
        return _coordinates.Contains(coordinates);
    }

    public void Hit()
    {
        if (!Sinks)
            HitCount++;
    }

    public override string ToString()
    {
        return string.Join("", _coordinates.Select(x => x));
    }
}