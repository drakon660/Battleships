using System.Xml.Linq;

namespace Battleships;

public interface IWarship
{
    Coordinate Start { get; }
    Direction Direction { get; }
    int Length { get; }
    bool CheckPosition(Coordinate coordinates);

    IReadOnlyList<Coordinate> Coordinates { get; }
}

public enum Direction
{
    South,
    East
}

public class Coordinate : IEquatable<Coordinate>
{
    public char Column { get; }
    public int Row { get; }

    private Coordinate(char column, int row)
    {
        Column = column;
        Row = row;
    }

    public static Coordinate Create(char column, int row) => new Coordinate(column, row);


    public bool Equals(Coordinate? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Column == other.Column && Row == other.Row;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Coordinate)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Column, Row);
    }
}

public sealed class Warship : IWarship
{
    public Coordinate Start { get; }
    public Direction Direction { get; }
    public int Length { get; }

    private List<Coordinate> _coordinates { get; } = new List<Coordinate>();
    
    public IReadOnlyList<Coordinate> Coordinates => _coordinates;

    private Warship(Coordinate start, Direction direction, int length)
    {
        Start = start;
        Direction = direction;
        Length = length;

        _coordinates.Add(start);

        if (direction == Direction.East)
        {
            int column = Start.Column;

            for (int i = 1; i < Length; i++)
            {
                column++;
                char nextCharacter = (char)column;
                _coordinates.Add(Coordinate.Create(nextCharacter, Start.Row));
            }
        }

        if (direction == Direction.South)
        {
            int row = Start.Row;
            for (int i = 1; i < Length; i++)
            {
                row++;
                _coordinates.Add(Coordinate.Create(start.Column, row));
            }
        }
    }

    public static Warship Create(Coordinate start, Direction direction, int length) =>
        new Warship(start, direction, length);

    public bool CheckPosition(Coordinate coordinates)
    {
        return _coordinates.Contains(coordinates);
    }
}

public class BattleShipBoard
{
    private List<IWarship> _warships = new List<IWarship>();

    public BattleShipBoard(int size)
    {
    }

    public FireResult Fire(Coordinate coordinates)
    {
        throw new NotImplementedException();
    }
}

public enum FireResult
{
    Shots,
    Misses,
    Sinks
}