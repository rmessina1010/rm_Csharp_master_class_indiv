using System.Text.Json;

void Runner(){
    char @continue;
    do{
        Console.WriteLine("Enter the name of the file you want to read");
        string fileName ;
        string fileContents;
        fileName = GetUserInput();
        fileContents = LoadData(fileName);
        var videoGames =  JsonSerializer.Deserialize<List<VideoGames>>(fileContents);
        DisplayGameData(videoGames);
        Console.WriteLine("Continue?");
        @continue = Console.ReadKey().KeyChar;
        Console.WriteLine();
     } while(char.ToUpper(@continue) == 'Y');
}


void Run(){
    try{
        Runner();
    }
    catch(NullReferenceException){
        Console.WriteLine("Null lists arent allowed!");
        Run();
    }
    catch(JsonException){
        Console.WriteLine("This file is not pa valid JSON file/format!");
        Run();
    }
    catch(FileNotFoundException){
        Console.WriteLine("This file doesn't exist!");
        Run();
    }
}

string GetUserInput(){
    string fileName ;        
    bool isValidFileName = false;
    do{
     fileName = Console.ReadLine() ?? "";
        if (fileName.Trim() == ""){ 
            Console.WriteLine("Filename cannot be blank!");
            continue;
            }
        if (!fileName.EndsWith(".json")){
            fileName+=".json";
        }
        isValidFileName = true;
            }while(!isValidFileName);
    return fileName;
}

string LoadData(string fileName, string path="jsonFiles/"){
    return File.ReadAllText(path+fileName);
}

void DisplayGameData(List<VideoGames> videoGames){
    if (videoGames.Count == 0) {
        Console.WriteLine("\nThis list is empty!");
        return;
    }
    Console.WriteLine("\nLoaded games are:");
    foreach(var game in videoGames){
        Console.WriteLine(game);
    }
}

Run();





public class VideoGames{
    public string? Title{get; init;}
    public int ReleaseYear{get; init;}
    public double Rating{get; init;}

    public override string ToString(){
        return $"{Title}, released in {ReleaseYear}. Rating:{Rating}";
    }
}
