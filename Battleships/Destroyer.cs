using Ardalis.GuardClauses;

namespace Battleships;

public sealed class Destroyer : IWarship
{
    public Destroyer(string name)
    {
        Name = name;
    }

    public static Destroyer Create(string name)
    { 
        Guard.Against.NullOrEmpty(name);

        return new Destroyer(name);
    }

    public string Name { get; }
    public int Length => 4;
}