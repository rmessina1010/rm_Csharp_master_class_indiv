using NUnit.Framework;
using Diceroll; 

namespace dicerollTest;
[TestFixture]
public class DiceTest
{

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
        Assert.AreEqual("Message", writer.ToString().Trim());    
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
            Assert.AreEqual("range error", writer.ToString().Trim());    
        }
    }
}    

[TestFixture]
public class UserInputTest{

}    
