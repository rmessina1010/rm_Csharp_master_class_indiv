// using QuoteFinder.UserInteraction;

public class Program{

    public static void Main(){
        var word = UserInteraction.ReadValidWord( "What word are you looking for?");
        var pages =  UserInteraction.ReadInteger( "How many pages do you want to read");
        var limit =  UserInteraction.ReadInteger( "How many quotes per page");
        // var multithred =  UserInteraction.ReadBool( "Use multi-threading [y/n]");
    }
}
public static class UserInteraction{
    public  static T GetInput<T>( string prompt = ""){
        if (prompt != "") { Console.WriteLine($"{prompt}?"); }
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

    public static string ReadValidWord(string prompt){
        Console.WriteLine(prompt);
        string word;
        do{
            word = Console.ReadLine();
        }while (!isValidWord(word));
        return word;
    }

    public static bool isValidWord( string input){
        return input is not null && input.Length>0 && input.All(char.IsLetter);
    }
    public static int ReadInteger(string message)
    {
        Console.WriteLine(message);
        int result;
        while (!int.TryParse(Console.ReadLine(), out result)){};
        return result;
    }

    public static bool ReadBool(string message)
    {
        Console.WriteLine($"{message} ('y' for 'yes', anything else for 'no')");
        var input = Console.ReadLine();
        return input == "y";
    }
}