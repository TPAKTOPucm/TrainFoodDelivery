﻿namespace TrainFoodDelivery.Models;

public class Recipe
{
    public int Id { get; set; }
    public int TrainNumber { get; set; }
    public Train Train { get; set; }
    public int WagonNumber { get; set; }
    public string Text { get; set; }
    public IEnumerable<Ingredient> Ingredients { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public IEnumerable<Photo> Photos { get; set; }
    public IEnumerable<Video> Videos { get; set; }
}
