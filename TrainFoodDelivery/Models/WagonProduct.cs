namespace TrainFoodDelivery.Models;

public class WagonProduct
{
    public int TrainNumber { get; set; }
    public Train Train { get; set; }
    public int WagonNumber { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int ProductAmount { get; set; }
}
