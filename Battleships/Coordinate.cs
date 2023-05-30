namespace Battleships;

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
    public static Coordinate Create(int column, int row) => new Coordinate((char)column, row);

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

    public override string ToString()
    {
        return $"{Column}{Row}";
    }
}