namespace TrainFoodDelivery.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public int? RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    public decimal Cost { get; set; }
}
