namespace TrainFoodDelivery.Models;

public class WagonProduct
{
    public int WagonId { get; set; }
    public Wagon Wagon { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int ProductAmount { get; set; }
}
