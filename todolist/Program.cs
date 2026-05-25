
string AppName = "TO DO:List 2026";
(string Label , char Char) exitPath =  (Label:"[E]xit", Char: 'E');
List<string> TODOList = new List<string>();

Dictionary<char, string> Operations = new Dictionary<char, string>
        {
            ['S'] =  "See all TODOs" ,
            ['A'] = "Add TODO",
            ['R'] = "Remove TODO",
        };
        
char OptionsMenu(){
    char userInput;
    Console.WriteLine($"\nWhat do you wish to do?");
    foreach(var (key, val) in Operations){
        string label = val[0] == key ? val.Substring(1)  : val;
        Console.WriteLine("[{0}]{1}", key, label);
    } 
    Console.WriteLine(exitPath.Label);
    userInput = Console.ReadKey().KeyChar;
    userInput= char.ToUpper(userInput);
    Console.WriteLine();
    return userInput;
}

bool IsValidTODO( string userInput){
    if (userInput.Trim() == string.Empty){
        Console.WriteLine("Error: Blank TODO's are not permited!");
        return false;
    }
    if (TODOList.Count()>0 && TODOList.Contains(userInput)){
        Console.WriteLine("Notice: This TODO already exists.");
        return false;
    }
    return true;
}

bool IsEmptyListMessage(){
    bool isEmptyList = TODOList.Count < 1;
    if (isEmptyList){ 
        Console.WriteLine("The TODO list is empty.");
    }
    return isEmptyList;
}


void AddTODO(){
        string userTODO;
        do{
            Console.WriteLine("Enter your TODO:");
            userTODO = Console.ReadLine() ?? "";
        }while (!IsValidTODO(userTODO));
        if (userTODO.ToUpper() == "E"){ return; }
        TODOList.Add(userTODO);
}

bool DisplayTODOs(){
    if (IsEmptyListMessage()) {return false;}
    int index = 1;
    foreach ( string TODO in TODOList){
            Console.WriteLine($"{index}) {TODO}");
            index++;
            }
            return true;
 }


void DeleteTODO(){
                bool inRange;
                int deleteIndex;
                do {
                    bool hasTODOs = DisplayTODOs();
                    if (!hasTODOs) {return;}
                    Console.WriteLine("Choose a To Do to delete:");
                    string userInput =  Console.ReadLine() ?? "";
                    if (userInput.Trim() == string.Empty){ return; }
                    deleteIndex =  int.Parse(userInput);
                    inRange = deleteIndex > 0 && deleteIndex <= TODOList.Count;
                    if (!inRange) { 
                        Console.WriteLine("Error: Out of range!");
                        return;
                    }
                }while(!inRange);
                TODOList.RemoveAt(deleteIndex-1);
}

void RunTODO(){
    char userChoice;
    do {
        userChoice = OptionsMenu();
        switch (userChoice){
            case 'S':
                DisplayTODOs();
            break;
            case 'A':
                AddTODO();
                break;
            case 'R':
                DeleteTODO();
                break;
            case  'E':
                break;    
            default:
                Console.WriteLine("Opps. That's not an option!");
                break;         
        }
    }while(userChoice != 'E');
    Console.WriteLine("Goodbye.");

}

Console.WriteLine(AppName);
RunTODO();