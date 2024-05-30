namespace TrainFoodDelivery.DTOs;

public class RecipeDto
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public int WagonNumber { get; set; }
    public IEnumerable<IngredientDto> Ingredients { get; set; }
    public int Amount { get; set; }
    public string Text { get; set; }
    public ProductDto Product { get; set; }
    public IEnumerable<string> Videos { get; set; }
    public IEnumerable<string> Photos { get; set; }
}
