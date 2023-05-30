// using FluentAssertions;
//
// namespace Battleships.Tests;
//
// public class WarshipTests
// {
//     [Fact]
//     public void Check_If_Worship_Is_Created_Correctly_On_East()
//     {
//         var warship = Battleship.Create(Coordinate.Create('D',2), Direction.East);
//         warship.Coordinates.Should().BeEquivalentTo(new List<Coordinate>()
//         {
//             Coordinate.Create('D',2),
//             Coordinate.Create('E',2),
//             Coordinate.Create('F',2),
//             Coordinate.Create('G',2),
//             Coordinate.Create('H',2),
//         });
//     }
//     
//     [Fact]
//     public void Check_If_Worship_Is_Created_Correctly_On_South()
//     {
//         var warship = Battleship.Create(Coordinate.Create('G',5), Direction.South);
//         warship.Coordinates.Should().BeEquivalentTo(new List<Coordinate>()
//         {
//             Coordinate.Create('G',5),
//             Coordinate.Create('G',6),
//             Coordinate.Create('G',7),
//             Coordinate.Create('G',8),
//             Coordinate.Create('G',9),
//         });
//     }
//     
//     [Fact]
//     public void Check_If_Worship_Is_On_Position()
//     {
//         var warship = Battleship.Create(Coordinate.Create('G',5), Direction.South);
//         bool isOnPosition = warship.CheckPosition(Coordinate.Create('G', 6));
//         isOnPosition.Should().BeTrue();
//     }
// }