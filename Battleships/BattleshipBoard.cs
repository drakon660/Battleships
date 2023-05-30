using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.JavaScript;
using Ardalis.GuardClauses;

namespace Battleships;

public interface ICoordinatesGenerator
{
    (int column, int row, Direction Direction) Generate();
}

public class StandardCoordinatesGenerator : ICoordinatesGenerator
{
    private readonly int _columnMin;
    private readonly int _columnMax;
    private readonly int _rowMin;
    private readonly int _rowMax;

    public StandardCoordinatesGenerator(int columnMin = 65, int columnMax = 74, int rowMin = 1, int rowMax = 10)
    {
        _columnMin = columnMin;
        _columnMax = columnMax;
        _rowMin = rowMin;
        _rowMax = rowMax;
    }

    public (int column, int row, Direction Direction) Generate()
    {
        Random random = new Random();

        int column = random.Next(_columnMin, _columnMax);
        int row = random.Next(_rowMin, _rowMax);

        var direction = (Direction)random.Next((int)Direction.South, (int)Direction.East);

        return (column, row, direction);
    }
}

public class BattleshipBoard
{
    private readonly ICoordinatesGenerator _coordinatesGenerator;
    private readonly int _size;
    private readonly List<IWarship> _warships = new();
    public IReadOnlyList<IWarship> Warships => _warships;
    private const int ColumnMin = 65;
    private const int ColumnMax = 74;
    private const int RowMin = 1;
    private const int RowMax = 10;

    public BattleshipBoard(ICoordinatesGenerator coordinatesGenerator)
    {
        _coordinatesGenerator = coordinatesGenerator;
    }

    public void Add(IWarship ship)
    {
        _warships.Add(ship);
    }

    public void Move(ref int column, ref int row, Direction direction, int length)
    {
        if (direction == Direction.East)
        {
            int difference = ColumnMax - column;
            if (difference < length)
                column -= (length - difference) - 1;
        }
        else
        {
            int difference = RowMax - row;
            if (difference < length)
                row -= (length - difference) - 1;
        }
    }

    public virtual void GenerateFirstShip(int length)
    {
        if (_warships.Count != 0) return;
        var (column, row, direction) = _coordinatesGenerator.Generate();

        Move(ref column, ref row, direction, length);

        if (length == 5)
            _warships.Add(Battleship.Create(Coordinate.Create((char)column, row), direction));
        else if (length == 4) _warships.Add(Destroyer.Create(Coordinate.Create((char)column, row), direction));
    }
    
    public virtual void GenerateRest(int length)
    {
        if (_warships.Count == 0) return;
        
        var (column, row, direction) = _coordinatesGenerator.Generate();

        foreach (var warship in _warships)
        {
            var start = warship.Start;
            int warshipRow = start.Row;
            int warshipRoColumn = start.Column;
            
            var coordinates = warship.Coordinates;
            
            
        }
        
        
        if (length == 5)
            _warships.Add(Battleship.Create(Coordinate.Create((char)column, row), direction));
        else if (length == 4) _warships.Add(Destroyer.Create(Coordinate.Create((char)column, row), direction));
    }

    public FireResult Fire(Coordinate coordinates)
    {
        Guard.Against.Zero(_warships.Count);

        var warshipHitOrNot = _warships.SingleOrDefault(x => x.CheckPosition(coordinates));
        if (warshipHitOrNot == null)
            return FireResult.Misses;

        warshipHitOrNot.Hit();

        return warshipHitOrNot.Sinks ? FireResult.Sinks : FireResult.Shots;
    }
}