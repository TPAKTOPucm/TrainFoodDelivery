namespace TrainFoodDelivery.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public IEnumerable<Ingredient> Ingredients { get; set; }
}
