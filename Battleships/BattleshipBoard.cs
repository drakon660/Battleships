namespace Battleships;

public class BattleshipBoard
{
    private readonly ICoordinatesGenerator _coordinatesGenerator;
    private readonly int Size = 10;
    private readonly Dictionary<int[], WarshipWithHits> _warships = new();
    public IReadOnlyDictionary<int[], WarshipWithHits> Warships => _warships.AsReadOnly();

    public BattleshipBoard(ICoordinatesGenerator coordinatesGenerator)
    {
        _coordinatesGenerator = coordinatesGenerator;
    }

    public void AddShip(IWarship warship, char column, int row, Direction direction)
        => AddShip(warship, (int)column, row, direction);

    public void AddShip(IWarship ship, int column, int row, Direction direction)
    {
        int position = (row - 1) * Size + column;

        if (direction == Direction.East)
        {
            int[] positions = Enumerable.Range(position, ship.Length).ToArray();
            _warships.Add(positions, WarshipWithHits.Create(ship, positions.Aggregate((x, y) => x + y)));
        }

        if (direction == Direction.South)
        {
            int[] positions = Enumerable.Range(position, ship.Length).Select(x => x + Size).ToArray();
            _warships.Add(positions, WarshipWithHits.Create(ship, positions.Aggregate((x, y) => x + y)));
        }
    }

    private int FindPosition(int column, int row) => (row - 1) * Size + column;

    public FireResult Fire(int column, int row)
    {
        int position = FindPosition(column, row);

        foreach (var warshipAndHitCount in _warships)
        {
            bool positionFound = Array.IndexOf(warshipAndHitCount.Key, position) != -1;
            if (!positionFound) continue;
            warshipAndHitCount.Value.DecreaseSumHitPosition(position);

            return warshipAndHitCount.Value.SumHitPosition == 0 ? FireResult.Sinks : FireResult.Shots;
        }

        return FireResult.Misses;
    }
}

public sealed class WarshipWithHits
{
    private IWarship Warship { get; }
    public int SumHitPosition { get; private set; }

    private WarshipWithHits(IWarship warship, int sumHitPosition)
    {
        Warship = warship;
        SumHitPosition = sumHitPosition;
    }

    public void DecreaseSumHitPosition(int value) => SumHitPosition -= value;

    public static WarshipWithHits Create(IWarship warship, int sumHitPosition)
        => new WarshipWithHits(warship, sumHitPosition);
}