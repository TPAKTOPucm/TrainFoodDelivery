namespace TrainFoodDelivery.Models;

public class OrderRecipe
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public int Amount { get; set; }
}
