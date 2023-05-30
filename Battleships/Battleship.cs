namespace Battleships;

public sealed class Battleship : Warship
{
    private Battleship(Coordinate start, Direction direction) : base(start, direction, 5)
    {
        
    }

    public static Battleship Create(Coordinate start, Direction direction) =>
        new Battleship(start, direction);

    public static Battleship Create(string coordinates, Direction direction)
    { 
       char column = coordinates[0];
       int row = coordinates[1];

       return new(Coordinate.Create(column,row), direction);
    }
}