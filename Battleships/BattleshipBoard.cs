using Ardalis.GuardClauses;

namespace Battleships;

public class BattleshipBoard
{
    private readonly ICoordinatesGenerator _coordinatesGenerator;
    private readonly int Size = 10;
    private readonly Dictionary<int, WarshipState> _warshipsMap = new();
    public IReadOnlyDictionary<int, WarshipState> WarshipsMap => _warshipsMap.AsReadOnly();
    public IEnumerable<IWarship> Warships => _warshipsMap.Select(x => x.Value.Warship);
 
    public BattleshipBoard(ICoordinatesGenerator coordinatesGenerator)
    {
        _coordinatesGenerator = coordinatesGenerator;
    }

    // public void AddShip(IWarship warship, char column, int row, Direction direction)
    //     => AddShip(warship, (int)column, row, direction);

    public void Generate()
    {
        if (!Warships.Any())
        {
            var startingPoint = _coordinatesGenerator.Generate();

            var destroyer = Destroyer.Create("Regan");
            
            Move(ref startingPoint.column, ref startingPoint.row, startingPoint.Direction, destroyer.Length);
            
            AddShip(destroyer, startingPoint.column, startingPoint.row, startingPoint.Direction);
        }
    }
    
    private void Move(ref int column, ref int row, Direction direction, int length)
    {
        if (direction == Direction.East)
        {
            int difference = Size - column;
            if (difference < length)
                column -= (length - difference) - 1;
        }
        else
        {
            int difference = Size - row;
            if (difference < length)
                row -= (length - difference) - 1;
        }
    }

    public void AddShip(IWarship ship, int column, int row, Direction direction)
    {
        int position = CalculateTargetPoint(column, row);
        
        if (direction == Direction.East)
        {
            var positions = Enumerable.Range(position, ship.Length).ToList();
            var warshipState = WarshipState.Create(ship, positions);
            foreach (var positionValue in positions)
            {
                _warshipsMap.Add(positionValue, warshipState);
            }
        }

        if (direction == Direction.South)
        {
            var positions = new List<int> { position };
            for (int i = 1; i < ship.Length; i++)
                positions.Add(positions[i-1] + 10);
            
            var warshipState = WarshipState.Create(ship, positions);
            foreach (var positionValue in positions)
            {
                _warshipsMap.Add(positionValue, warshipState);
            }
        }
    }

    private int CalculateTargetPoint(int column, int row) => (row - 1) * Size + column;

    public FireResult Fire(int column, int row)
    {
        int position = CalculateTargetPoint(column, row);

        if (!_warshipsMap.TryGetValue(position, out var warshipState)) 
            return FireResult.Misses;
       
        warshipState.Damage(position);

        return warshipState.IsDestroyed ? FireResult.Sinks : FireResult.Shots;
    }
}

public struct WarshipState
{
    public IWarship Warship { get; }
    private readonly List<int> _placePoints; 
    public bool IsDestroyed => _placePoints.Sum(x => x) == 0;

    // private WarshipState()
    // {
    //         
    // }
    //
    
    private WarshipState(IWarship warship, List<int> placePoints)
    {
        Warship = warship;
        _placePoints = placePoints;
    }
    
    public void Damage(int placePoint) => _placePoints.Remove(placePoint);

    public static WarshipState Create(IWarship warship, List<int> placePoints) =>
        new WarshipState(warship, placePoints);
}