using Output.TableOutput;
using Output.UserMenu;
using ApiData;
using SWLogic;

public class Program{
    public static async Task Main()
    {

        IApiDataReader apiDataReader = new ApiDataReader("https://swapi.dev/api/");
        var apiJsonData = await apiDataReader.Read("https://swapi.dev/api/planets/?format=json");
        Root apiData = EncodeHander.JSONDeserialize(apiJsonData);
        var swPlanets = new StarWarsData(apiData);

        Console.WriteLine("--- A Galaxy Far Far Away ---");
        var printer = new TablePrinter<StarWarsRow>(swPlanets.TableData);
        printer.PrintTable();

        var userInputMenu = new UserMenu();
        char userChoice;
        do{
           userChoice= userInputMenu.RequestOperation();
           var  maxPlanet = PerformSWLogic.getMinMaxSW(userInputMenu.Operations[userChoice].Col, swPlanets.TableData);
           var  minPlanet = PerformSWLogic.getMinMaxSW(userInputMenu.Operations[userChoice].Col, swPlanets.TableData, false);
           userInputMenu.WriteStats(userChoice, maxPlanet, minPlanet);
        } while (userInputMenu.exitPath.Char != userChoice);
    }
}
