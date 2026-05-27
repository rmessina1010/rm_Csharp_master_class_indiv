// using Ingredient = (int Id, string Name, string Instruction);
// using Recipe = System.Collections.Generic.List<int>;
// using RecipeList = System.Collections.Generic.List<Recipe>;
using System.Text.Json;

public class Program{
    public static void Main(){
        var savedRecipeList = FileHander.LoadRecipes();
        var Book  = new RecipeHandler (savedRecipeList);
        int itemCount = Book.MenuList.Count;
        char exit;
        do{
            Console.WriteLine("Recipe Book 2026!!");
        UserInteraction.DisplayAllRecipes(Book.RecipeBook, Book.MenuList);
        UserInteraction.DisplayMenu("\n\nCreate a new cookie recipe! Select an Ingredient to add:", Book.MenuList);;
        bool isValid = false;
        do{
            ( isValid,   int selection) = UserInteraction.GetUserSelection(itemCount);
            if (isValid){
                Book.AddIngredientToRecipe(selection);
            }
        }while (isValid);
        UserInteraction.PrintRecipe( Book.AddRecipeToBook(), Book.MenuList);
        Console.WriteLine("Exit?");
        exit = char.ToUpper(Console.ReadKey().KeyChar);
        }while (exit != 'Y');
        string saveableData = EncodeHander.JSONSerialize(Book.RecipeBook);
        FileHander.Save(saveableData);
    }
}


public class RecipeList : List<List<int>> 
{
    public void AddRow(List<int> row) => this.Add(row);
}
public interface IMenuContent{
    List <(int Id, string Name, string Instruction)> MenuList{ get; }
}


public interface IRecipeBook{
    public List<int> AddRecipeToBook();
    RecipeList RecipeBook {get;}
}
public class RecipeHandler: IMenuContent, IRecipeBook{
    
    public  List <(int Id, string Name, string Instruction)> MenuList  { get; } = new() 
    {
        (1, "Wheat Flour",  "Sieve. Add to other ingredients."),
        (2, "Coconut Flour",  "Sieve. Add to other ingredients."),
        (3, "Butter",  "Melt on low heat. Add to other ingredients."),
        (4, "Chocolate",  "Melt in a water bath. Add to other ingredients."),
        (5, "Sugar",  "Add to other ingredients."),
        (6, "Cardamom",  "Take half a teaspoon. Add to other ingredients."),
        (7, "Cinnamon",  "Take half a teaspoon. Add to other ingredients."),
        (8, "Cocoa powder",  "Add to other ingredients."),
    };
    
    public RecipeList RecipeBook { get; private set;} = new ();

    List<int> CurrentRecipe = new();

    public  void AddIngredientToRecipe(int Id){
        CurrentRecipe.Add(Id);
        Console.WriteLine($"Ingredient, {MenuList[Id-1].Name}, Added");
    }

    public List<int> AddRecipeToBook(){
        List<int> recipe = [.. CurrentRecipe];
        RecipeBook.Add(recipe);
        CurrentRecipe.Clear();
        Console.WriteLine("Recipe Added");
        return recipe;
    }

    public   RecipeHandler(RecipeList recipes ){
        if (recipes.Count > 0){ RecipeBook = recipes; }
    }

}

public class UserInteraction {

    public  static (bool, int) GetUserSelection( int menuItems){
        Console.WriteLine("Add an ingredient by its ID or type anything else if finished.");
        string userInput = Console.ReadLine() ?? "";
        (bool isValid, int choice) = Validation.InRange( userInput, 1, menuItems);
        return  (isValid, choice);
    }

     public static void DisplayMenu(string label, List<(int Id, string Name, string Instruction)> menuContent){
        Console.WriteLine(label);
        foreach(var ingredient in menuContent){
            Console.WriteLine($"{ingredient.Id}) {ingredient.Name}: {ingredient.Instruction}");
        } 
    }

    public static void DisplayAllRecipes( List<List<int>> recipeList,List<(int Id, string Name, string Instruction)> IngredientRef ){
            if (recipeList.Count == 0){
                Console.WriteLine("No Recipes.");
                return;
            }
                int recipeIndex =1;
                Console.WriteLine("Existing Recipes are:\n");
            foreach(List<int> recipe in recipeList){
                Console.WriteLine($"-***** {recipeIndex} *****-");
                PrintRecipe(recipe, IngredientRef);
                recipeIndex++;
            }

            }
        public static void PrintRecipe(List<int> recipe, List<(int Id, string Name, string Instruction)> InredeientRef){
                            foreach(int ingredient  in recipe){
                    Console.WriteLine($" {InredeientRef[ingredient-1].Name}: {InredeientRef[ingredient-1].Instruction}");
                }

        }    
    }



public static class Validation{
    public static (bool, int) IsInt(string input, string message=""){
        bool isInt = int.TryParse(input,out int result);
         if (message != ""  && !isInt){ Console.WriteLine(message); }
        return (isInt, result);
    }

    public static (bool, int) InRange(string input, int low, int high, string message="", string subErr=""){
        (bool isInt, int result) = IsInt(input, subErr);
        if (isInt == true){ 
            if (result >= low && result <= high){ return (true, result);}
            if (message != "" ){ Console.WriteLine(message);}
        }    
        return (false, result);
    } 
}

public   static  class EncodeHander {
    public static string JSONSerialize(RecipeList list){
        return JsonSerializer.Serialize(list);
    }
    public static RecipeList JSONDeserialize(string data){
        return JsonSerializer.Deserialize<RecipeList>(data)?? new RecipeList();
    }
}


public   static  class FileHander {
    public static void Save(string content, string path="recipe_data.txt"){
        File.WriteAllText(path, content);
    }
    public static string Read( string path){
        string data = "";
        if (File.Exists(path)){
           data = File.ReadAllText(path);
        }
        return data;
    }

    public static RecipeList LoadRecipes(string path="recipe_data.txt"){
        string data =  Read(path);
        if (data.Trim() == string.Empty){ data="[]"; }
        RecipeList recipeList;
        recipeList = EncodeHander.JSONDeserialize(data);
        return recipeList;
    }


}