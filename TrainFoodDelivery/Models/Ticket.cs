namespace TrainFoodDelivery.Models;
public enum UserRole
{
    Admin = 0,
    Cooker = 1,
    Deliverer = 2,
    Customer = 3
}

public class Ticket
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public UserRole Role { get; set; }
    public int TrainNumber { get; set; }
    public Train Train { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    public int WagonNumber { get; set; }
    public int SeatNumber { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
}
