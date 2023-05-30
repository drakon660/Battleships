using Ardalis.GuardClauses;

namespace Battleships;

public sealed class Battleship : IWarship
{
    private Battleship(string name)
    {
        Name = name;
    }
    public static Battleship Create(string name)
    {
        Guard.Against.NullOrEmpty(name);
        return new Battleship(name);
    }
    public string Name { get; }
    public int Length => 5;
}