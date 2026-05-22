using System;
using System.Threading;

namespace Diceroll{
    public class Program{
        public static void Main(string[] args){
        //    Die @die = new Die(6);
        //    int rolls = 120;
        //    for (int i=0; i<rolls; i++){
        //         @die.Roll();
        //    }
        //     int side =0;
        //    foreach (int @count in @die._apperances){
        //         side++;
        //         Console.WriteLine( $"side {side} happened {@count} times out of {rolls}");
        //    }
        new Game(3,6).Play();
           

    }
}

public class Die{
    public int Sides {  get; init; }
    public  int[] _apperances {private set; get;}

    public int OnFace{ get; private set;} 
     private Random Randomness;

    public Die (int sides){
        Sides = sides;
        _apperances = new int[sides];
        Randomness = Random.Shared;
    }
    
    public int Roll(){
        int roll =  Randomness.Next(Sides);
        _apperances[roll]++;
        OnFace = roll+1;
        return OnFace ;
   }
    public float Probability (){ return 1f/(float)Sides; }
}

public class Game {
    public int Tries { get; init; }
    public Die @Die { get; init; }

    public Game(int tries, int sides){
        Tries = tries;
        @Die = new Die(sides);
    }

    public void Play(){

        int playerChoice;
        Player player = new Player();
        bool again = true;
        bool win;
        UserInput @UserInput = new UserInput(new Dictionary<string, string>{
            {"notNumber" , "Enter a valid integer number!"},
            {"notRange"  , "This choice is not in range!"}
        });

        while (again){
            win = false;
            @Die.Roll();
            for (int i = Tries; i > 0; i--){
                playerChoice = @UserInput.Range(1,@Die.Sides, $"Pick a number between 1 and {@Die.Sides}, {player.Name}");
                if (@Die.OnFace == playerChoice){
                Console.WriteLine($"OMG, {player.Name}, you won!!");
                    win = true;
                    break;
                }
                if (i-1 > 0){ Console.WriteLine($"{i-1} guess{ (i-1 != 1 ? "es":"")} left..."); }
            }
            if (!win) { Console.WriteLine($"The number was {@Die.OnFace}. You lost!");}
            again =  PlayAgain(player.Name);
        }
        Console.WriteLine($"OK. Goodbye, {player.Name}");

    }

    private bool PlayAgain(string name, int delay = 2000){
        Thread.Sleep(delay);
        char choice;
        Console.WriteLine($"Would you like to play again, {name},[y/n]?");
        do{
            choice = char.ToUpper(Console.ReadKey().KeyChar);
        }while (choice != 'Y' && choice != 'N' ) ;
        Console.WriteLine();
        return choice == 'Y';
    }
    
}

public class UserInput{

        private Dictionary<string,string> ErrorMessages {get; init;}

        public UserInput(Dictionary<string, string> errors){
            string[] props = {"notNumber", "notRange"};
            ErrorMessages = new Dictionary<string, string>();
            var errKeys  = errors.Keys;
            foreach (string prop in props){
                ErrorMessages.Add(prop, errors[prop] ?? "");
            }
        }

        public int Range(int number1, int number2, string label){
        string userInput;
        int sanitized;
        bool isValid;
        do{
            Console.WriteLine(label);
            userInput = Console.ReadLine() ?? "";
            isValid = Validation.InRange(userInput, number1,number2,out int result,ErrorMessages["notRange"], ErrorMessages["notNumber"] );
            sanitized = result;
        }while(!isValid);
        return sanitized;
    }
    public int Number(string label){
        string userInput;
        int sanitized;
        bool isValid;
        do{
            Console.WriteLine(label);
            userInput = Console.ReadLine() ?? "";
            isValid = Validation.IsInt(userInput, out int result, ErrorMessages["notNumber"]);
            sanitized = result;
        }while(!isValid);
        return sanitized;
    }
}

public static class Validation{
    public static bool IsInt(string input, out int validInput, string message=""){
        bool isInt = int.TryParse(input,out int result);
        validInput = result;
        if (message != ""  && !isInt){ Console.WriteLine(message); }
        return isInt;
    }

    public static bool InRange(string input, int low, int high, out int validInput, string message="", string subErr=""){
        bool isInt = IsInt(input, out int result,subErr);
        validInput = result;
        if (!isInt){ return false;} 
        if (result >= low && result <= high){ return true;}
        if (message != "" ){ Console.WriteLine(message);}
        return false;
        }
    
}

public class Player {

    public string Name {  get; init; }

    public Player(){
        string name;
        do{
            Console.WriteLine("Hello there. What is your name?");
            name = (Console.ReadLine() ?? "").Trim();
        } while( name == "");
        Name = name;
    }
}
}