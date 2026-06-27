using NUnit.Framework;
using  FibonacciGenerator;
namespace fibonacciTest;


[TestFixture]
public class FibonacciTest
{

    
    [TestCase (new int[]{0,1,1,2,3,5,8}, 7)]
    [TestCase (new int[]{0}, 1)]
    [TestCase (new int[0], 0)]
    public void Generates_Valid_Fib_Sequences( int[] exp, int input){
        var result = Fibonacci.Generate(input);
        Assert.AreEqual(exp, result.ToArray());
    }
    
    [Test]
    public void Generates_Valid_Fib_Sequence_At_Max_1134903170(){
        int maxFibValue = 1134903170;
        int maxFibNumber = 46;
        var result = Fibonacci.Generate(46);
        Assert.AreEqual(maxFibValue, result.ToArray()[maxFibNumber - 1]);
    }

    [TestCase (-1)]
    [TestCase (-10)]
    [TestCase (-100)]
    public void Throws_ArgumentException_for_Neg(int n){
          Assert.Throws<ArgumentException>(()=>Fibonacci.Generate(n));
    }
    
    [TestCase (47)]
    [TestCase (100)]
    [TestCase (1000)]
    public void Throws_ArgumentException_for_gt_MaxValidNumber(int n){
         Assert.Throws<ArgumentException>(()=>Fibonacci.Generate(n));
    }


}


