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

    [TestCase (-1)]
    [TestCase (-10)]
    public void Throws_ArgumentException_for_Neg_gt_MaxValidNumber(int n){
         Assert.Throws<ArgumentException>(()=>Fibonacci.Generate(n));
         Assert.Throws<ArgumentException>(()=>Fibonacci.Generate(Fibonacci.MaxValidNumber+1));
    }

}


