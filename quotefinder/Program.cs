// using QuoteFinder.UserInteraction;

public class Program{

    public static void Main(){
        var word = UserInteraction.GetInput<string>( "What word are you looking for");
        var pages =  UserInteraction.GetInput<int>( "How many pages do you want to read");
        var limit =  UserInteraction.GetInput<int>( "How many quotes per page");
        // var multithred =  UserInteraction.GetInput<char>( "Use multi-threading [y/n]");
    }

}
public static class UserInteraction{
    public  static T GetInput<T>( string prompt){
        Console.WriteLine($"{prompt}?");
        if (typeof (T) == typeof(char)){
            return (T)(object)Console.ReadKey().KeyChar;
        }
        string rawInput= Console.ReadLine();
        if (typeof (T) == typeof(string)){
            return (T)(object) rawInput;
        }
        if (typeof (T) == typeof(int)){
            int.TryParse(rawInput, out int anInt);
            return (T)(object) anInt;
        }     
        if (typeof (T) == typeof(double)){
            return (T)(object) double.Parse(rawInput);
        }        
        throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
    }

}