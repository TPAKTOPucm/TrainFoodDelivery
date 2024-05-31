namespace TrainFoodDelivery.DTOs;

public class NearestWagonsInfo
{
    public int Amount { get; set; }
    public IEnumerable<int> Number { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public string Description { get; set; }
    public NearestWagonsInfo NearestWagons { get; set; }
    public int? RecipeId { get; set; }
    public RecipeDto Recipe { get; set; }
    public decimal Cost { get; set; }
    public decimal OneAmount { get; set; }
    public string VolumeType { get; set; }
    public int Amount { get; set; }
}
