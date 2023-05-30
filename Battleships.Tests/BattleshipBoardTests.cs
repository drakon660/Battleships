using System.Buffers;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Moq;

namespace Battleships.Tests;

public class BattleshipBoardTests
{
    [Fact]
    public void Check_If_Fire_Can_Miss_The_Ship()
    {
       var battleshipBoard =  new BattleshipBoard(new StandardCoordinatesGenerator());
       
       battleshipBoard.Add(Battleship.Create(Coordinate.Create('C',3),Direction.South));
       
       var result = battleshipBoard.Fire(Coordinate.Create('C',2));

       result.Should().Be(FireResult.Misses);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_The_Ship()
    {
        var battleshipBoard =  new BattleshipBoard(new StandardCoordinatesGenerator());
       
        battleshipBoard.Add(Battleship.Create(Coordinate.Create('F',7),Direction.East));
       
        var result = battleshipBoard.Fire(Coordinate.Create('G',7));

        result.Should().Be(FireResult.Shots);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_The_Sinks()
    {
        var battleshipBoard =  new BattleshipBoard(new StandardCoordinatesGenerator());
       
        battleshipBoard.Add(Battleship.Create(Coordinate.Create('F',7),Direction.East));
       
        battleshipBoard.Fire(Coordinate.Create('F',7));
        battleshipBoard.Fire(Coordinate.Create('G',7));
        battleshipBoard.Fire(Coordinate.Create('H',7));
        battleshipBoard.Fire(Coordinate.Create('I',7));
        var result = battleshipBoard.Fire(Coordinate.Create('J',7));
        //var result = battleshipBoard.Fire(Coordinate.Create('G',7));

        result.Should().Be(FireResult.Sinks);
    }

    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_No_Edge()
    {
        var coordinator = new Mock<ICoordinatesGenerator>();

        coordinator.Setup(x=>x.Generate()).Returns((65,1,Direction.East));
        
        var battleshipBoard =  new BattleshipBoard(coordinator.Object);
        battleshipBoard.GenerateFirstShip(5);

        var warships = new List<IWarship>
        {
            Battleship.Create(Coordinate.Create(65, 1), Direction.East)
        };
        
        battleshipBoard.Warships.Should().BeEquivalentTo(warships);
    }
    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_Column_Edge()
    {
        var coordinator = new Mock<ICoordinatesGenerator>();
        
        coordinator.Setup(x=>x.Generate()).Returns(('I',6,Direction.East));
        
        var battleshipBoard =  new BattleshipBoard(coordinator.Object);
        battleshipBoard.GenerateFirstShip(5);

        var warships = new List<IWarship>
        {
            Battleship.Create(Coordinate.Create('F', 6), Direction.East)
        };
        
        battleshipBoard.Warships.Should().BeEquivalentTo(warships);
    }
    
    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_Row_Edge()
    {
        var coordinator = new Mock<ICoordinatesGenerator>();
        
        coordinator.Setup(x=>x.Generate()).Returns(('C',8,Direction.South));
        
        var battleshipBoard =  new BattleshipBoard(coordinator.Object);
        battleshipBoard.GenerateFirstShip(5);

        var warships = new List<IWarship>
        {
            Battleship.Create(Coordinate.Create('C', 6), Direction.South)
        };
        
        battleshipBoard.Warships.Should().BeEquivalentTo(warships);
    }

    [Fact]
    public void Check_If_Next_Ship_Is_Generated_Successfully()
    {
        var coordinator = new Mock<ICoordinatesGenerator>();
        
        coordinator.Setup(x=>x.Generate()).Returns(('C',8,Direction.South));
        
        var battleshipBoard =  new BattleshipBoard(coordinator.Object);
        battleshipBoard.Add(Battleship.Create("H8", Direction.South));
        battleshipBoard.Add(Destroyer.Create("C9", Direction.South));
        
        battleshipBoard.GenerateRest(1);
    }
}