namespace Battleships.Tests;

public class CharToIntConverterTests
{
    [Fact]
    public void Check_If_Converting_From_Char_To_Int_Is_Working()
    {
        char a = 'A';
        int numberOfA = a;
        Assert.Equal(65,numberOfA);
    }
    
    [Fact]
    public void Check_If_Converting_From_Int_To_Char_Is_Working()
    {
        int numberOfA = 65;
        char a = (char)numberOfA;
        Assert.Equal('A',a);
    }
}