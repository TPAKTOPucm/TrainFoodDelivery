using Microsoft.EntityFrameworkCore;
using TrainFoodDelivery.Data;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.Repository;

public class RecipeRepository : IRecipeRepository
{
    private readonly TrainFoodDeliveryContext _db;
    public RecipeRepository(TrainFoodDeliveryContext db)
    {
        _db = db;
    }
    public async Task Cook(int id)
    {
        var orderRcipe = await _db.OrderRecipes.FindAsync(id);
        if (await _db.OrderRecipes.Where(or => or.OrderId == orderRcipe.OrderId).CountAsync() == 1) {
            var order = await _db.Orders.FindAsync(orderRcipe.OrderId);
            order.Status = OrderStatus.Cooked;
            _db.Update(order);
        }
        _db.Remove(orderRcipe);
        await _db.SaveChangesAsync();
    }

    public Task CreateRecipe(RecipeDto recipe)
    {
        _db.Add(new Recipe
        {
            TrainNumber = recipe.TrainNumber,
            WagonNumber = recipe.WagonNumber,
            ProductId = recipe.Product.Id,
            Ingredients = recipe.Ingredients.Select(i => new Ingredient
            {
                Id = i.Id,
                Name = i.Name,
                Amount = i.Amount
            })
        });
        return _db.SaveChangesAsync();
    }

    public async Task DeleteRecipe(int recipeId)
    {
        var recipe = await _db.Recipes.FindAsync(recipeId);
        _db.Remove(recipe);
        await _db.SaveChangesAsync();
    }

    public Task<List<RecipeDto>> GetCookOrders(int trainNumber, int wagonNumber)  =>
        _db.OrderRecipes.Where(or => or.Recipe.TrainNumber == trainNumber && or.Recipe.WagonNumber == wagonNumber).Select(or => new RecipeDto
        {
            Id = or.Id,
            Amount = or.Amount,
            Text = or.Recipe.Text,
            Ingredients = or.Recipe.Ingredients.Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name
            }),
            Product = new ProductDto
            {
                Id = or.Recipe.ProductId,
                Name = or.Recipe.Product.Name,
                ImagePath = or.Recipe.Product.ImagePath
            }
        }).ToListAsync();
    /*
    {
        var recipe = new RecipeDto
        {
            Id = 1,
            Amount = 4,
            Product = new ProductDto
            {
                Id = 1,
                Name = "Каша"
            },
        };
        var list = new List<RecipeDto>();
        list.Add(recipe);
        var ingredients = new List<IngredientDto>();
        ingredients.Add(new IngredientDto()
        {
            Id = 1,
            Name = "Крупа"
        });
        ingredients.Add(new IngredientDto()
        {
            Id = 2,
            Name = "Молоко"
        });
        recipe.Ingredients = ingredients;
        return Task.FromResult(list);
    }*/

    public Task<List<IngredientDto>> GetIngredients(int trainNumber, int wagonNumber) =>
        _db.Ingredients.Where(i => i.TrainNumber == trainNumber && i.WagonNumber == wagonNumber).Select(i => new IngredientDto
        {
            Id = i.Id,
            Name = i.Name,
            Amount = i.Amount
        }).ToListAsync();

    public Task<List<RecipeDto>> GetRecipes(int trainNumber, int wagonNumber) =>
        _db.Recipes.Where(r => r.TrainNumber == trainNumber && r.WagonNumber == wagonNumber).Select(r => new RecipeDto
        {
            Id = r.Id,
            Product = new ProductDto
            {
                Id = r.ProductId,
                Name = r.Product.Name,
                Description = r.Product.Description
            },
            Ingredients = r.Ingredients.Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name
            })
        }).ToListAsync();

    public Task SetIngredients(IEnumerable<IngredientDto> ingredients, int trainNumber, int wagonNumber)
    {
        throw new NotImplementedException();
    }
}
