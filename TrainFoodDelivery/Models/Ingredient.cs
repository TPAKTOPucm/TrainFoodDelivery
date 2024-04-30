namespace TrainFoodDelivery.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Amount { get; set; }
    public int WagonNumber { get; set; }
    public int TrainNumber { get; set; }
    public Train Train { get; set; }
    public IEnumerable<Recipe> Recipes { get; set; }
}
