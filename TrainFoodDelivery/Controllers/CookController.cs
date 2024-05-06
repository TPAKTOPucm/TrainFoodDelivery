using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TrainFoodDelivery.Controllers.Utils;
using TrainFoodDelivery.DTOs;
using TrainFoodDelivery.Models;
using TrainFoodDelivery.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TrainFoodDelivery.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CookController : ControllerBase
{
    private readonly ControllerUtils _utils;
    private readonly IDistributedCache _cache;
    private readonly IRecipeRepository _repository;
    public CookController(IRecipeRepository repository, IDistributedCache cache, ControllerUtils utils)
    {
        _utils = utils;
        _repository = repository;
        _cache = cache;
    }
    public async Task<IActionResult> AddRecipe(string jwt, int ticketIndex, RecipeDto recipe)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        await _repository.CreateRecipe(recipe);
        return Ok();
    }

    public async Task<IActionResult> GetRecipes(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        var recipes = await _repository.GetRecipes(ticket.TrainNumber, ticket.WagonNumber);
        return Ok(recipes);
    }

    public async Task<IActionResult> DeleteRecipe(string jwt, int ticketIndex, int recipeId)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        await _repository.DeleteRecipe(recipeId);
        return Ok();
    }

    public async Task<IActionResult> SetIngredients(string jwt, int ticketIndex, IEnumerable<IngredientDto> ingredients)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        await _repository.SetIngredients(ingredients, ticket.TrainNumber, ticket.WagonNumber);
        return Ok();
    }

    public async Task<IActionResult> GetIngredients(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        var ingredients = await _repository.GetIngredients(ticket.TrainNumber, ticket.WagonNumber);
        return Ok(ingredients);
    }

    public async Task<IActionResult> GetCookOrders(string jwt, int ticketIndex)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        var key = "CO" + ticket.TrainNumber + "_" + ticket.WagonNumber;
        var json = await _cache.GetStringAsync(key);
        if (json is null)
        {
            json = JsonSerializer.Serialize(await _repository.GetCookOrders(ticket.TrainNumber, ticket.WagonNumber));
            _cache.SetStringAsync(key, json);
        }
        return Ok(json);
    }

    public async Task<IActionResult> Cook(string jwt, int ticketIndex, int cookOrderId)
    {
        var ticket = await _utils.CheckIfAlowed(jwt, ticketIndex, UserRole.Cooker);
        if (ticket is null)
            return Forbid();
        await _repository.Cook(cookOrderId);
        await _cache.RemoveAsync("CO" + ticket.TrainNumber + "_" + ticket.WagonNumber);
        return Ok();
    }
}
