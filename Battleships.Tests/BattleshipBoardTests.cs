using System.Buffers;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Moq;

namespace Battleships.Tests;

public class BattleshipBoardTests
{
    [Fact]
    public void Check_If_Ship_Is_Added_East_Successfully()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.East);

        battleshipBoard.Warships.Count().Should().Be(1);
        battleshipBoard.Warships.First().Should().BeEquivalentTo(
        
            new int[]
            {
                22,23,24,25,26 
            }
        );
        
        // battleshipBoard.Warships.First().Value.Should().BeEquivalentTo(
        //
        //     WarshipWithHits.Create(Battleship.Create("Kwasniewski"), 0)
        // );
        // battleshipBoard.Warships.Should().BeEquivalentTo(new Dictionary<int[], IWarship>()
        // {
        //     {
        //         new int[]
        //         {
        //             22, 23, 24, 25, 26
        //         },
        //         Battleship.Create("Kwasniewski")
        //     }
        // }.AsReadOnly());
    }
    [Fact]
    public void Check_If_Ship_Is_Added_South_Successfully()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.South);

        // battleshipBoard.Warships.Count.Should().Be(1);
        // battleshipBoard.Warships.First().Key.Should().BeEquivalentTo(
        //
        //     new int[]
        //     {
        //         32,33,34,35,36 
        //     }
        // );
        //
        // battleshipBoard.Warships.First().Value.Should().BeEquivalentTo(
        //
        //     (Battleship.Create("Kwasniewski"), 0)
        // );
        // battleshipBoard.Warships.Should().BeEquivalentTo(new Dictionary<int[], IWarship>()
        // {
        //     {
        //         new int[]
        //         {
        //             22, 23, 24, 25, 26
        //         },
        //         Battleship.Create("Kwasniewski")
        //     }
        // }.AsReadOnly());
    }

    [Fact]
    public void Check_Generate()
    {
        for (var i = 0; i < 100; i++)
        {
            var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
            battleshipBoard.Generate();
            battleshipBoard.Generate();    
        }
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_String_Coordinates_The_Ship()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),"D5", Direction.East);
        var result = battleshipBoard.Fire("F5");
        result.Should().Be(FireResult.Shots);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Miss_The_Ship()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.East);
        var result = battleshipBoard.Fire(1, 3);
        result.Should().Be(FireResult.Misses);
    }

    [Fact]
    public void Check_If_Fire_Can_Hit_The_Ship()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.South);
        battleshipBoard.Fire(2, 3);
        battleshipBoard.Fire(2, 4);
        battleshipBoard.Fire(2, 5);
        battleshipBoard.Fire(2, 6);
        battleshipBoard.Fire(2, 6);
        battleshipBoard.Fire(2, 6);
        battleshipBoard.Fire(2, 6);
        battleshipBoard.Fire(2, 6);
        var result = battleshipBoard.Fire(2, 7);
        result.Should().Be(FireResult.Sinks);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_The_Sinks()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.South);
        battleshipBoard.Fire(2, 3);
        battleshipBoard.Fire(2, 4);
        battleshipBoard.Fire(2, 5);
        var result = battleshipBoard.Fire(2, 6);
        //var result = battleshipBoard.Fire(2, 7);
        result.Should().Be(FireResult.Sinks);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_The_Sinks_East()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.East);
        battleshipBoard.Fire(2, 3);
        battleshipBoard.Fire(3, 3);
        battleshipBoard.Fire(4, 3);
        battleshipBoard.Fire(5, 3);
        var result = battleshipBoard.Fire(6, 3);
        result.Should().Be(FireResult.Sinks);
    }
    
    [Fact]
    public void Check_If_Fire_Can_Hit_The_Sinks_South()
    {
        var battleshipBoard = new BattleshipBoard(new StandardCoordinatesGenerator());
        battleshipBoard.AddShip(Battleship.Create("Kwasniewski"),2,3, Direction.South);
        battleshipBoard.Fire(2, 3);
        battleshipBoard.Fire(2, 4);
        battleshipBoard.Fire(2, 5);
        battleshipBoard.Fire(2, 6);
        var result = battleshipBoard.Fire(2, 7);
        result.Should().Be(FireResult.Sinks);
    }

    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_No_Edge()
    {
        
    }
    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_Column_Edge()
    {
       
    }
    
    [Fact]
    public void Check_If_First_Ship_Is_Generated_Successfully_Row_Edge()
    {
      
    }

    [Fact]
    public void Check_If_Next_Ship_Is_Generated_Successfully()
    {
        
    }
}