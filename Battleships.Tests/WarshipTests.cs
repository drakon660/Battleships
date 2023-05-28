using FluentAssertions;

namespace Battleships.Tests;

public class WarshipTests
{
    [Fact]
    public void Check_If_Worship_Is_Created_Correctly_On_East()
    {
        var warship = Warship.Create(Coordinate.Create('D',2), Direction.East, 4);
        warship.Coordinates.Should().BeEquivalentTo(new List<Coordinate>()
        {
            Coordinate.Create('D',2),
            Coordinate.Create('E',2),
            Coordinate.Create('F',2),
            Coordinate.Create('G',2),
        });
    }
    
    [Fact]
    public void Check_If_Worship_Is_Created_Correctly_On_South()
    {
        var warship = Warship.Create(Coordinate.Create('G',5), Direction.South, 4);
        warship.Coordinates.Should().BeEquivalentTo(new List<Coordinate>()
        {
            Coordinate.Create('G',5),
            Coordinate.Create('G',6),
            Coordinate.Create('G',7),
            Coordinate.Create('G',8),
        });
    }
    
    [Fact]
    public void Check_If_Worship_Is_On_Position()
    {
        var warship = Warship.Create(Coordinate.Create('G',5), Direction.South, 4);
        bool isOnPosition = warship.CheckPosition(Coordinate.Create('G', 6));
        isOnPosition.Should().BeTrue();
    }
}