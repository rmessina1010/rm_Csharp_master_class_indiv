using NUnit.Framework;

using passwordgen;

namespace passwordgenTest;

[TestFixture]
public class PaswordGeneratorTest
{
    private PasswordGenerator _cut =  new PasswordGenerator(new RandomWrapper());
    private PasswordGenerator _cut2 =  new PasswordGenerator(new MockPasswordGeneration());
    
    [TestCase(5,10)]
    [TestCase(5,13)]
    [TestCase(3,26)]
    public void Generate_PasswordWithALength_WithinTheGivenRange(int min, int max){
        int passwordLength= _cut.Generate(min, max, false).Length;
        Assert.That(passwordLength, Is.InRange(min, max));
    }


    [TestCase(0,26)]
    [TestCase(-2,12)]
    public void Generate_ThrowsOutofRangeExcpetion_IfMinLessThan1(int min, int max){
        var ex = Assert.Throws<ArgumentOutOfRangeException>(()=>_cut.Generate(min, max, false), "no exception");
        Assert.That(ex.Message, Does.Contain("greater than 0"));
    }

    
    [TestCase(10,5)]
    [TestCase(10,7)]
    public void Generate_ThrowsOutofRangeExcpetion_IfMaxLessThanMin(int min, int max){
        var ex = Assert.Throws<ArgumentOutOfRangeException>(()=>_cut.Generate(min, max, false));
        Assert.That(ex.Message, Does.Contain("must be smaller than"));
    }

    public void Generate_PasswordWithALength_Of50(){
        int passwordLength= _cut.Generate(50, 50, false).Length;
        Assert.That(50, Is.EqualTo(passwordLength));
        
    }


// flakey test
    [Test]
    public void Generate_PasswordContains_SpecialCharacters(){
        string pasword= _cut.Generate(20000, 20000, true);
        Assert.That(pasword, Does.Match(@"[!@#$%^&*()_+=-]"));
    }
    [Test]
    public void Generate_PasswordDoesNotContain_SpecialCharacters(){
        string pasword= _cut.Generate(20000, 20000, false);
        Assert.That(pasword, Does.Not.Match(@"[!@#$%^&*()_+=-]"));
    }
    
    [Test]
    public void Generate_PasswordContains_SpecialCharacters_notflakey(){
        string pasword= _cut2.Generate(15, 15, true);
        Assert.That(pasword, Does.Match(@"[!@#$%^&*()_+=-]"));
    }
}

public class MockPasswordGeneration : IRandom{
    private readonly int[] _sequence = [0,1,2,43,42,5,6,7,8,9,10,11,12,13,14];
    private int _currentIndex = 0;
    public int Next  (int min, int max){
        return _sequence.Length;
    }
    public  int Next  (int max){
        if (_currentIndex >= _sequence.Length)
        {
            throw new InvalidOperationException("End of sequence reached.");
        }
        int nextValue = _sequence[_currentIndex];
        _currentIndex++; 
        return nextValue;    
    }
}
