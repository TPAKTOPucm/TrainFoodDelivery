namespace TrainFoodDelivery.Models;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }
}
