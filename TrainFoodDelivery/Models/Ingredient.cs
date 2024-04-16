namespace TrainFoodDelivery.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public int WagonId { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
