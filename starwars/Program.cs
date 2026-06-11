using Output.TableOutput;
using Output.UserMenu;
using ApiData;

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

    (string Name, int Val, string UnitName) getMinMaxSW(string prop, StarWarsRow[] data, bool isMax = true){
        Func<StarWarsRow, int>? selector = prop switch
        {
            "surface_water"   => planet => planet.surface_water,
            "population"      => planet => planet.population,
            "diameter"        => planet => planet.diameter,
            "rotation_period" => planet => planet.rotation_period,
            _                 => null
        };

        if (selector == null || data == null || data.Length == 0) { return ("No Planet", -2, ""); }        
            StarWarsRow selected = isMax ? 
                    data.MaxBy(selector) 
                    : data.MinBy(selector);
        return prop switch            
        {
            "surface_water"   => ( selected.name, selected.surface_water, "% coverage"),
            "population"      => (  selected.name,  selected.population, "people"),
            "diameter"        => ( selected.name, selected.diameter, "kilometers"),
            "rotation_period" => (  selected.name, selected.rotation_period, "hours"),
            _                 => ("No Planet", -2, "")
        };                    
       }
        string prop = "population";
        var  maxPlanet= getMinMaxSW(prop, swPlanets.TableData);
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
