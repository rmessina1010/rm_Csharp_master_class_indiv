// using QuoteFinder.UserInteraction;
using System.Net.Http;
using Models.Datum;
using System.Text.Json;

public class Program{

    public static async Task Main(){
        var word = UserInteraction.ReadValidWord( "What word are you looking for?");
        var pages =  UserInteraction.ReadInteger( "How many pages do you want to read");
        var limit =  UserInteraction.ReadInteger( "How many quotes per page");
        // var multithred =  UserInteraction.ReadBool( "Use multi-threading [y/n]");clear

        Console.WriteLine("Fetching data...");
        List<string> data =  await FetchDataFromAllPagesAsync(pages, limit);
        Console.WriteLine("Data is ready");
        
    var  ai = new QuoteListProcessor();
    Console.WriteLine(ai.ContainsWord("I went to Lola with my mum and my mum met everyone", word)? "yes":"no");

    }

    public static async Task<List<string>> FetchDataFromAllPagesAsync( int pages, int limit){
        var tasks = new Task<string>[pages];
        var quotesApiDataReader = new  QuotesApiDataReader();
        for (int i =0; i < pages ; i++){
            tasks[i] = quotesApiDataReader.Read(i+1, limit);
        }
        // Task.WaitAll(tasks); // is blocking;
        // return tasks.Select( tasks => tasks.Result).ToList(); 

        return (await Task.WhenAll(tasks)).ToList(); // note needs: WhenAll is awaited, returns  array
    }
}
public static class UserInteraction{
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

public class QuotesApiDataReader: IQuotesApiDataReader{
    private HttpClient _httpClient = new HttpClient();

    public async Task<string> Read (int page, int limit){
        string endpoint = $"https://quotegarden.onrender.com/api/v3/quotes?limit={limit}&page={page}";
        HttpResponseMessage  response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}

public interface IQuotesApiDataReader{
    Task<string> Read (int page , int limit);
}

public class QuoteListProcessor{
    private  Datum[] _deserializedDatums;
    private  char[] _splitArray = new [] {' ', '.',',',';','!','?',':'};
    public  async void DeserializeDataList(List<string> data){
        int listSize = data.Count();
        _deserializedDatums = new Datum[listSize];
        for( int i=0; i < listSize; i++ ){
            _deserializedDatums[i] = JsonSerializer.Deserialize<Datum>(data[i]);
        }
    }

    public bool ContainsWord (string input, string requiredWord){
        var split = input.Split(_splitArray, StringSplitOptions.RemoveEmptyEntries);
        return split.Any(word => string.Equals(word, requiredWord, StringComparison.OrdinalIgnoreCase));
    }

}