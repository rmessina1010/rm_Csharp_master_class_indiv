// using QuoteFinder.UserInteraction;
using System.Net.Http;
using Models.Datum;
using System.Text.Json;

public class Program{

    public static async Task Main(){
        
        var interactor = new UserInteraction();
        var quoteListProcessor = new QuoteListProcessor(interactor);
        var word  =  interactor.ReadValidWord( "What word are you looking for?");
        var pages = interactor.ReadInteger( "How many pages do you want to read");
        var limit = interactor.ReadInteger( "How many quotes per page");
        // var multithred =  UserInteraction.ReadBool( "Use multi-threading [y/n]");clear

        Console.WriteLine("Fetching data...");
        List<string> data =  await FetchDataFromAllPagesAsync(pages, limit);
        Console.WriteLine("Data is ready");

        //Console.WriteLine( quoteListProcessor.ContainsWord("'Age' is the acceptance of a term of years. But maturity is the glory of years.", word)? "yes":"no");
        quoteListProcessor.DeserializeDataList(data);
        quoteListProcessor.ProcessAllPages(word);
    }

    public static async Task<List<string>> FetchDataFromAllPagesAsync( int pages, int limit){
        var tasks = new Task<string>[pages];
        var quotesApiDataReader = new  QuotesApiDataReader();
        for (int i =0; i < pages ; i++){
            tasks[i] = quotesApiDataReader.Read(i+1, limit);
        }
        // Task.WaitAll(tasks); // is a blocking operation;
        // return tasks.Select( tasks => tasks.Result).ToList(); 

        return (await Task.WhenAll(tasks)).ToList(); // note needs: WhenAll is awaited, returns  array
    }
}
public class UserInteraction : IUserInteractor{
    public string ReadValidWord(string prompt){
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
    public int ReadInteger(string prompt)
    {
        Console.WriteLine(prompt);
        int result;
        while (!int.TryParse(Console.ReadLine(), out result)){};
        return result;
    }

    public bool ReadBool(string message)
    {
        Console.WriteLine($"{message} ('y' for 'yes', anything else for 'no')");
        var input = Console.ReadLine();
        return input == "y";
    }

    public void PrintMessage(string message){
        Console.WriteLine(message);
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

public interface IUserInteractor{
    void PrintMessage ( string Message);
}

public class QuoteListProcessor{
    private  Root[] _deserializedRoots;
    private IUserInteractor _interactor;
    private  char[] _splitArray = new [] {' ', '.',',',';','!','?',':','\''};

    public QuoteListProcessor(IUserInteractor interactor){
        _interactor = interactor;
    }
    public  async void DeserializeDataList(List<string> data){
        int listSize = data.Count();
        _deserializedRoots = new Root[listSize];
        for( int i=0; i < listSize; i++ ){
            _deserializedRoots[i] = JsonSerializer.Deserialize<Root>(data[i]);
        }
    }
    
    public void ProcessAllPages( string word){
        for (int i = 0, l = _deserializedRoots.Count(); i < l; i++ ){
            ProcessPage(i+1, word);
        }
    }

    public void ProcessPage(int page, string word){

        var pageCt = _deserializedRoots.Count();
        var root = !( pageCt < 1  || page < 1 || page > pageCt) ?
             _deserializedRoots[page - 1]
             : null ;
        var quoteWithWord = root.data?
            .Where( quoteWithWord => (ContainsWord(quoteWithWord.quoteText, word)))
            .MinBy(quote => quote.quoteText.Length);
        if (quoteWithWord is not null){
            _interactor.PrintMessage($"{page}) {quoteWithWord.quoteText} -- {quoteWithWord.quoteAuthor}");
        }
        else{
            _interactor.PrintMessage($"No quotes found on page {page}.");
        }

    }

    public bool ContainsWord (string input, string requiredWord){
        var split = input.Split(_splitArray, StringSplitOptions.RemoveEmptyEntries);
        return split.Any(word => string.Equals(word, requiredWord, StringComparison.OrdinalIgnoreCase));
    }

}