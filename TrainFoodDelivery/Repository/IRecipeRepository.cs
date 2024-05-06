using TrainFoodDelivery.DTOs;

namespace TrainFoodDelivery.Repository;

public interface IRecipeRepository
{
    public Task CreateRecipe(RecipeDto recipe);
    public Task DeleteRecipe(int recipeId);
    public Task<List<RecipeDto>> GetRecipes(int trainNumber, int wagonNumber);
    public Task<List<RecipeDto>> GetCookOrders(int trainNumber, int wagonNumber);
    public Task Cook(int id);
    public Task SetIngredients(IEnumerable<IngredientDto> ingredients, int trainNumber, int wagonNumber);
    public Task<List<IngredientDto>> GetIngredients(int trainNumber, int wagonNumber);
}
