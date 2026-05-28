using MenuPath = (string Label , char Char);

string AppName = "Calculator 2026";
MenuPath exitPath =  (Label:"[E]scape", Char: 'E');
MenuPath memoryPath =  (Label:"<M> key to access Memory", Char: 'M');
var  Operations = new Dictionary<char, (char Opp, string Label, Func<float, float, float> Foo)>
        {
            ['A'] = (Opp: '+', Label: "Add", Foo: (a,b) => a + b  ),
            ['S'] = (Opp: '-', Label: "Substract" , Foo: (a,b) => a - b ),
            ['D'] = (Opp: '/', Label: "Divide", Foo: (a,b) => a / b ),
            ['M'] = (Opp: '*', Label: "Multiply" , Foo: (a,b) => a * b ),
            ['R'] = (Opp: '%', Label: "Modulus", Foo: (a,b) => a % b  ),
        };
     
float NumberInput(string order, float memory){
    Console.WriteLine($"Input the {order} number:");
    string? userInput = Console.ReadLine();
    if (userInput == null || userInput.Trim() == string.Empty ) { return 0; }
    if (char.ToUpper(userInput[0]) == memoryPath.Char) {return  memory; }
    return float.Parse(userInput);
}

char RequestOperation(string post=""){
    char userInput;
    do{
        Console.WriteLine($"What do you wish to do{post}?");
        foreach(var (key, val) in Operations){
            string label = val.Label[0] == key ? val.Label.Substring(1)  : val.Label;
            Console.WriteLine("[{0}]{1}", key, label);
        } 
        Console.WriteLine(exitPath.Label);
        userInput = Console.ReadKey().KeyChar;
        userInput= char.ToUpper(userInput);
        Console.WriteLine();
    }while(!(userInput == exitPath.Char || Operations.ContainsKey(userInput)));
    return userInput;
}

// float RunOperation(char operation, float a, float b){
//     switch (operation){
//         case  '+':
//             return a + b ;
//         case  '-':
//             return a - b ;
//         case  '*':
//             return  a * b;
//         case  '/': 
//             return a/b; 
//         case  '%': 
//             return a%b; 
//         default:
//             return 0;
//     }
// }

float RunCalculator( float memory = 0, bool isStart = true){
    char operation = RequestOperation(isStart ? "" : " next");
    if (operation == exitPath.Char) { 
         Console.WriteLine($"Thank you for using {AppName}. Until next time!");
         return memory;
    }
    if (Operations.ContainsKey(operation)){
        float number1 = NumberInput ("first", memory);
        float number2 = NumberInput ("second", memory);
        memory = Operations[operation].Foo(number1, number2);
        Console.WriteLine("{0} {1} {2} = {3}", number1, Operations[operation].Opp,number2, memory);
    }
    else{ Console.WriteLine("Invalid option");}    
    return RunCalculator(memory, false);
};

Console.WriteLine(AppName);
RunCalculator();
