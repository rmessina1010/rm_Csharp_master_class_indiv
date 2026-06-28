using NUnit.Framework;
using Diceroll; 

namespace dicerollTest;
[TestFixture]
public class DiceTest
{

    private Die _cut;
    private int Sides;

    [SetUp]
    public void SetUp(){
        _cut= new Die(6);
        Sides = 6;
    }
    

    [TestCase(3)]
    [TestCase(5)]
    public void Die_Roll_Returns_Given_Side_When_InRange(int? expect){
         Assert.AreEqual(expect.Value, _cut.Roll(expect.Value));
    }

    [TestCase(7)]
    [TestCase(0)]
    [TestCase(null)]
    public void Die_Roll_Returns_Random_Valid_Side_When_Not_InRange_Or_Null(int? target){
         Assert.That(_cut.Roll(target),Is.InRange(1, Sides));
    }
    
    [Test]
    public void Die_Roll_Returns_Random_Valid_Side_When_No_Args_Passed(){
         Assert.That(_cut.Roll(),Is.InRange(1, Sides));
    }

    [Test]
    public void Apperance_Tracks_Die_Rolls(){
        _cut.Roll(2);
        _cut.Roll(2);
        _cut.Roll(2);
        _cut.Roll(5);
        Assert.AreEqual(3,_cut._appearances[1]);
        Assert.AreEqual(1,_cut._appearances[4]);
    }
}


[TestFixture]
public class ValidationTest{

    [TestCase("-1", -1)]
    [TestCase("-11", -11)]
    [TestCase("0", 0)]
    [TestCase("1", 1)]
    [TestCase("5", 5 )]
    [TestCase("10", 10)]
    public void IsInt_Returns_True_for_Ints(string input, int expect){
        using var writer = new StringWriter();
        bool isValidInt = Validation.IsInt(input, out int res, "Message", writer);
        Assert.True(isValidInt);
        Assert.True(  expect == res);
        Assert.AreEqual("", writer.ToString().Trim());    

    }
    
    [TestCase("Ray")]
    [TestCase("Tiffianne")]
    [TestCase("K8")]
    public void IsInt_Returns_False_for_NonInts(string input){
        using var writer = new StringWriter();
        bool isValidInt = Validation.IsInt(input, out int res, "Message", writer);
        Assert.False(isValidInt);
        Assert.AreEqual(0,res);
        Assert.That( writer.ToString(), Does.Contain("Message")); 
    }
        
    [TestCase("5",1,6,true,5)]
    [TestCase("1",1,6,true,1)]
    [TestCase("6",1,6,true,6)]
    [TestCase("7",1,6,false,7)]
    [TestCase("V",1,6,false,0)]
 
     public void InRange_Returns_True_WithinRange_Otherwise_False(string input, int lo, int hi, bool expect, int expectInt){
        using var writer = new StringWriter();
        bool isInRange = Validation.InRange(input, lo, hi, out int validRes, "range error", "int error", writer);
        Assert.AreEqual(expect, isInRange);    
        Assert.AreEqual(expectInt,validRes);
        if (!isInRange && validRes != 0){
            // Assert.AreEqual("range error", writer.ToString().Trim());   
            Assert.That( writer.ToString(), Does.Contain("range error"));  
        }
    }
}    

[TestFixture]
public class UserInputTest{

[TestCase("Ray")]
[TestCase("Veronica")]
    public void Player_Construct_Gets_Name( string expected){
    var input = new StringReader(expected); 
    Console.SetIn(input);
    var _cut = new Player();
    Assert.AreEqual(expected, _cut.Name);
    }
}    
