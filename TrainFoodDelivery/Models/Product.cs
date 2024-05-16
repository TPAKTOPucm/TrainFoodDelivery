namespace TrainFoodDelivery.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public IEnumerable<WagonProduct> WagonProducts { get; set; }
    public Recipe? Recipe { get; set; }
    public decimal Cost { get; set; }
}
