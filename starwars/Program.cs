using Output.TableOutput;
using Output.UserMenu;
using ApiData;
using SWLogic;

public class Program{
    public static async Task Main()
    {
        // Creating your test data
        Starship[] fleet = new Starship[]
        {
            new Starship { Name = "X-Wing", Class = "Starfighter", Crew = 1, IsHyperdriveCapable = true },
            new Starship { Name = "Millennium Falcon", Class = "Light Freighter", Crew = 2, IsHyperdriveCapable = true },
            new Starship { Name = "TIE Fighter", Class = "Starfighter", Crew = 1, IsHyperdriveCapable = false },
            new Starship { Name = "Death Star", Class = "Battle Station", Crew = 342953, IsHyperdriveCapable = true }
        };

        IApiDataReader apiDataReader = new ApiDataReader("https://swapi.dev/api/");
        var apiJsonData = await apiDataReader.Read("https://swapi.dev/api/planets/?format=json");
        Root apiData = EncodeHander.JSONDeserialize(apiJsonData);
        var swPlanets = new StarWarsData(apiData);

        string prop = "population";
        var  maxPlanet = PerformSWLogic.getMinMaxSW(prop, swPlanets.TableData);
        Console.WriteLine($"Max {prop}, {maxPlanet.Name}, with {maxPlanet.Val} {maxPlanet.UnitName}");
        

        // Test Case 1: Print all columns
        Console.WriteLine("--- Full Fleet Report ---");
        var printer = new TablePrinter<StarWarsRow>(swPlanets.TableData);
        printer.PrintTable();

        // Test Case 2: Print only specific columns (Whitelist)
        Console.WriteLine("\n--- Name and Class Only ---");
        string[] filter = { "Name", "Class" };
        var filteredPrinter = new TablePrinter<Starship>(fleet, filter);
        filteredPrinter.PrintTable();
        var userInputMenu = new UserMenu();
        char userChoice = userInputMenu.RequestOperation();
        if (userInputMenu.Operations.ContainsKey(userChoice)){
            Console.WriteLine($"you chose {fleet[0].GetType().GetProperty(userInputMenu.Operations[userChoice].Col)?.GetValue(fleet[0], null)}");
        }
    }
}
public struct Starship
{
    public string Name { get; set; }
    public string Class { get; set; }
    public int Crew { get; set; }
    public bool IsHyperdriveCapable { get; set; }
}
