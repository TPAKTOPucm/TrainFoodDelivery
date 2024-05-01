namespace TrainFoodDelivery.DTOs;

public class RecipeDto
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public int WagonNumber { get; set; }
    public IEnumerable<IngredientDto> Ingredients { get; set; }
    public ProductDto Product { get; set; }
}
