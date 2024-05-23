namespace TrainFoodDelivery.Models;

public enum OrderStatus
{
    Ordering = 0,
    Ordered = 1,
    Cooked = 2,
    Delivering = 3,
    Delivered = 4,
    Completed = 5,
    Cancelled = 6
}
public enum PaymentType
{
    Карта = 0,
    Наличные = 1
}

public class Order
{
    public int Id { get; set; }
    public IEnumerable<ProductOrder> Products { get; set; }
    public int TicketId { get; set; }
    public PaymentType? PaymentType { get; set; }
    public Ticket Ticket { get; set; }
    public OrderStatus Status { get; set; }
}
