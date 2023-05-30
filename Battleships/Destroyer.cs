namespace Battleships;

public sealed class Destroyer : Warship
{
    private Destroyer(Coordinate start, Direction direction) : base(start, direction, 4)
    {
        
    }
    
    public static Destroyer Create(Coordinate start, Direction direction) =>
        new Destroyer(start, direction);
    
    public static Destroyer Create(string coordinates, Direction direction)
    { 
        char column = coordinates[0];
        int row = coordinates[1];

        return new(Coordinate.Create(column,row), direction);
    }
}