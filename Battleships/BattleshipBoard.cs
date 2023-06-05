using Ardalis.GuardClauses;

namespace Battleships;

public class BattleshipBoard
{
    private readonly ICoordinatesGenerator _coordinatesGenerator;
    private readonly int Size = 10;
    private readonly Dictionary<int, WarshipState> _warshipsMap = new();
    public IReadOnlyDictionary<int, WarshipState> WarshipsMap => _warshipsMap.AsReadOnly();
    public IEnumerable<IWarship> Warships => _warshipsMap.Select(x => x.Value.Warship).Distinct();

    public BattleshipBoard(ICoordinatesGenerator coordinatesGenerator)
    {
        _coordinatesGenerator = coordinatesGenerator;
    }

    public void Generate()
    {
        if (!Warships.Any())
        {
            var startingPoint = _coordinatesGenerator.Generate();
            var regan = Destroyer.Create("Regan");
            Move(ref startingPoint.column, ref startingPoint.row, startingPoint.Direction, regan.Length);
            AddShip(regan, startingPoint.column, startingPoint.row, startingPoint.Direction);
        }
        else
        {
            var kennedy = Destroyer.Create("Kennedy");
            var kennedyCodes = FindFreeSpace(kennedy.Length);
            AddShip(kennedy, kennedyCodes);

            var obama = Battleship.Create("Obama");
            var obamaCodes = FindFreeSpace(obama.Length);
            AddShip(kennedy, obamaCodes);
        }
    }

    public List<int> FindFreeSpace(int length)
    {
        bool notFoundSpace;
        var loopIterator = 0;
        List<int> codes;
        do
        {
            var startingPoint = _coordinatesGenerator.Generate();
            Move(ref startingPoint.column, ref startingPoint.row, startingPoint.Direction, length);
            codes = GenerateMapCodes(startingPoint.column, startingPoint.row, length,
                startingPoint.Direction);

            notFoundSpace = codes.Any(code => _warshipsMap.ContainsKey(code));

            loopIterator++;

            if (loopIterator == 100)
                break;
        } while (notFoundSpace);

        return codes;
    }

    public List<int> BoundariesFromCodes(List<int> codes)
    {
        List<int> boundaries = new List<int>();
        int minValue = codes.First();
        int maxValue = codes.Last();
        
        int lowerBound = --minValue;
        int upperBound = ++maxValue;
        
        int count = codes.Count + 2;
        
        var lowerBoundaries = Enumerable.Range(lowerBound - Size, count);
        var upperBoundaries = Enumerable.Range(upperBound + Size - (codes.Count + 1), count);
        
        boundaries.AddRange(lowerBoundaries);
        boundaries.AddRange(upperBoundaries);
        boundaries.Add(minValue);
        boundaries.Add(maxValue);
        
        return boundaries;
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

    public void AddShip(IWarship ship, List<int> positions)
    {
        try
        {
            var warshipState = WarshipState.Create(ship, positions);
            foreach (var positionValue in positions)
            {
                _warshipsMap.Add(positionValue, warshipState);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public (List<int> Codes, List<int> Boundaries) GenerateMapCodesWithBoundaries(int column, int row, int length, Direction direction)
    {
        int position = CalculateTargetPoint(column, row);

        List<int> codes = new List<int>();
        List<int> boundaries = new List<int>();

        if (direction == Direction.East)
            codes = Enumerable.Range(position, length).ToList();

        if (direction == Direction.South)
        {
            codes = new List<int> { position };

            for (int i = 1; i < length; i++)
                codes.Add(codes[i - 1] + 10);
        }
        
        int lowerBound = codes.First();
        int upperBound = codes.Last();
        
        if (row == 1)
        {
            int count = codes.Count;

            if (column > 1 && column < 10)
            {
                count++;
                lowerBound--;
            }

            var upperBoundaries = Enumerable.Range(lowerBound, count);
            boundaries.AddRange(upperBoundaries);
            
            if (column == Size)
            {
                
            }
        }

        return (codes, boundaries);
    }
    
    public List<int> GenerateMapCodes(int column, int row, int length, Direction direction)
    {
        int position = CalculateTargetPoint(column, row);

        List<int> positions = new List<int>();

        if (direction == Direction.East)
            positions = Enumerable.Range(position, length).ToList();

        if (direction == Direction.South)
        {
            positions = new List<int> { position };

            for (int i = 1; i < length; i++)
                positions.Add(positions[i - 1] + 10);
        }

        return positions;
    }

    public List<int> GenerateMapCodes(string positionToPlace, int length, Direction direction)
    {
        var stringToColumnRow = StringToColumnRow(positionToPlace);
        int position = CalculateTargetPoint(stringToColumnRow.Column, stringToColumnRow.Row);

        List<int> positions = new List<int>();

        if (direction == Direction.East)
            positions = Enumerable.Range(position, length).ToList();

        if (direction == Direction.South)
        {
            positions = new List<int> { position };

            for (int i = 1; i < length; i++)
                positions.Add(positions[i - 1] + 10);
        }

        return positions;
    }

    public void AddShip(IWarship ship, string positionToPlace, Direction direction) =>
        AddShip(ship, GenerateMapCodes(positionToPlace, ship.Length, direction));
    
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
                positions.Add(positions[i - 1] + 10);

            var warshipState = WarshipState.Create(ship, positions);
            foreach (var positionValue in positions)
            {
                _warshipsMap.Add(positionValue, warshipState);
            }
        }
    }

    private int CalculateTargetPoint(int column, int row) => (row - 1) * Size + column;

    public (int Column, int Row) StringToColumnRow(string coordinates)
    {
        int difference = 'A' - 1;

        int column = coordinates[0] - difference;
        int row = coordinates[1];

        return (column, row);
    }

    public FireResult Fire(string positionToHit)
    {
        var stringToColumnRow = StringToColumnRow(positionToHit);

        return Fire(stringToColumnRow.Column, stringToColumnRow.Row);
    }

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