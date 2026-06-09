using Output.TableOutput;


public class Program{
    public static void Main()
    {
        // Creating your test data
        Starship[] fleet = new Starship[]
        {
            new Starship { Name = "X-Wing", Class = "Starfighter", Crew = 1, IsHyperdriveCapable = true },
            new Starship { Name = "Millennium Falcon", Class = "Light Freighter", Crew = 2, IsHyperdriveCapable = true },
            new Starship { Name = "TIE Fighter", Class = "Starfighter", Crew = 1, IsHyperdriveCapable = false },
            new Starship { Name = "Death Star", Class = "Battle Station", Crew = 342953, IsHyperdriveCapable = true }
        };

        // Test Case 1: Print all columns
        Console.WriteLine("--- Full Fleet Report ---");
        var printer = new TablePrinter<Starship>(fleet);
        printer.PrintTable();

        // Test Case 2: Print only specific columns (Whitelist)
        Console.WriteLine("\n--- Name and Class Only ---");
        string[] filter = { "Name", "Class" };
        var filteredPrinter = new TablePrinter<Starship>(fleet, filter);
        filteredPrinter.PrintTable();
    }
}
public struct Starship
{
    public string Name { get; set; }
    public string Class { get; set; }
    public int Crew { get; set; }
    public bool IsHyperdriveCapable { get; set; }
}
