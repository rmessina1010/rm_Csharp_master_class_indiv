using MenuPath = (string Label , char Char);

namespace Output.UserMenu{    
public class UserMenu{
    public string Prompt = Environment.NewLine+"Select the statistics you are interested in:";
    public Dictionary<char, (string Col, string Label)> Operations = new Dictionary<char, (string Col, string Label)>
        {
            ['P'] = (Col: "population", Label: "Population"),
            ['D'] = (Col: "diameter" , Label: "Diameter"),
            ['S'] = (Col: "surface_water", Label: "Surface Water"),
        };
    public  MenuPath exitPath =  (Label:"[E]scape", Char: 'E');

    public char RequestOperation(){
        char userInput;
        do{
            Console.WriteLine(Prompt);
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
}


}