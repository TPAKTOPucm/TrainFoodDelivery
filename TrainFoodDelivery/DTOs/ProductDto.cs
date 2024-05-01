namespace TrainFoodDelivery.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public int NearestWagon { get; set; }
    public int? RecipeId { get; set; }
    public RecipeDto Recipe { get; set; }
    public decimal Cost { get; set; }
}
