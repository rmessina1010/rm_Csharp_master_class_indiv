namespace CookieCookbook.Recipes.Ingredients;

public class IngredientsRegister : IIngredientsRegister
{
    public IEnumerable<Ingredient> All { get; } = new List<Ingredient>
    {
        new WheatFlour(),
        new SpeltFlour(),
        new Butter(),
        new Chocolate(),
        new Sugar(),
        new Cardamom(),
        new Cinnamon(),
        new CocoaPowder()
    };

    public Ingredient GetById(int id)
    {
        var ingredientsWithAGivenId = All
            .Where(ingredient => ingredient.Id == id);
        if (ingredientsWithAGivenId.Count > 1)   {
            throw new InvalidOperationExeption($"More than one ingredient has the ID of {$id}");
        }
        // if ( All.Select(ingredient => ingredient.Id).Distinct().Count() is idCount != All.Count() is count){
        //     throw new InvalidOperationExeption($"There are { count - idCount } duplicate IDs in the data");
        // }
        return ingredientsWithAGivenId.FirstOrDefault(ingredient => ingredient.Id == id);
    }
}

