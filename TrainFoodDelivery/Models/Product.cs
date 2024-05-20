namespace TrainFoodDelivery.Models;
using System.ComponentModel.DataAnnotations.Schema;

public enum NettoType
{
    л = 0,
    шт = 1,
    г = 2,
    мл = 3,
    кг = 4
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public IEnumerable<WagonProduct> WagonProducts { get; set; }
    public Recipe? Recipe { get; set; }
    public decimal Cost { get; set; }
    public decimal Netto { get; set; }
    public NettoType NettoType { get; set; }
}
