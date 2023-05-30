namespace Battleships;

public interface ICoordinatesGenerator
{
    (int column, int row, Direction Direction) Generate();
}