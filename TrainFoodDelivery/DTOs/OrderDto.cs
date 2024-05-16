using TrainFoodDelivery.Models;

namespace TrainFoodDelivery.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
    public int? TicketId { get; set; }
    public TicketDto? Ticket { get; set; }
    public OrderStatus Status { get; set; }
}
