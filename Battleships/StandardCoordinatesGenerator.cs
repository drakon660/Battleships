namespace Battleships;

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