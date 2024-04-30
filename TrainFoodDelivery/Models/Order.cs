namespace TrainFoodDelivery.Models;

public class Order
{
    public int Id { get; set; }
    public IEnumerable<ProductOrder> Products { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}
