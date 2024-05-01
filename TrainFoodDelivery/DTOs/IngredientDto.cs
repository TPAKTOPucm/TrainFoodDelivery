namespace TrainFoodDelivery.DTOs;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public int WagonNumber { get; set; }
    public int TrainNumber { get; set; }
    public IEnumerable<RecipeDto> Recipes { get; set; }
}
